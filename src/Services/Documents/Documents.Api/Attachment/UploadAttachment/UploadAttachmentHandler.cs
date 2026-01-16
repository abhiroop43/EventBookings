namespace Documents.Api.Attachment.UploadAttachment;

public record UploadAttachmentCommand(string FileName, string FileContent, string ContentType)
    : ICommand<UploadAttachmentResult>;

public record UploadAttachmentResult(bool IsSuccess, string FileId, string? ErrorMessage = null);

public class UploadAttachmentCommandValidator : AbstractValidator<UploadAttachmentCommand>
{
    public UploadAttachmentCommandValidator()
    {
        RuleFor(x => x).NotNull();
        RuleFor(x => x.FileName).NotEmpty().WithMessage("File Name is required");
        RuleFor(x => x.FileContent).NotEmpty().WithMessage("File Content is required");
    }
}

public class UploadAttachmentCommandHandler(
    ILogger<UploadAttachmentCommandHandler> logger,
    IOptionsSnapshot<S3Configuration> configuration) : ICommandHandler<UploadAttachmentCommand, UploadAttachmentResult>
{
    private IAmazonS3 _s3Client;

    public async Task<UploadAttachmentResult> Handle(UploadAttachmentCommand command,
        CancellationToken cancellationToken)
    {
        var clientConfig = new AmazonS3Config
        {
            ServiceURL = configuration.Value.ServiceUrl,
            ForcePathStyle = true,
            UseHttp = true
        };

        _s3Client =
            new AmazonS3Client(new BasicAWSCredentials(configuration.Value.AccessKey, configuration.Value.SecretKey),
                clientConfig);
        var bucketCreated = await CreateBucketAsync(configuration.Value.Bucket);

        if (!bucketCreated)
        {
            logger.LogError("Failed to create bucket with name: {BucketName}", configuration.Value.Bucket);
            return new UploadAttachmentResult(false, string.Empty, "Failed to create bucket");
        }

        PutObjectRequest request = new()
        {
            BucketName = configuration.Value.Bucket,
            Key = command.FileName,
            InputStream = new MemoryStream(Convert.FromBase64String(command.FileContent)),
            ContentType = command.ContentType
        };

        var response = await _s3Client.PutObjectAsync(request, cancellationToken);

        if (response.HttpStatusCode == HttpStatusCode.OK) return new UploadAttachmentResult(true, command.FileName);

        logger.LogError("Failed to upload file, Status Code: {StatusCode}", response.HttpStatusCode);
        return new UploadAttachmentResult(false, string.Empty,
            $"Failed to upload file, Status Code: {response.HttpStatusCode}");
    }

    private async Task<bool> CreateBucketAsync(string bucketName)
    {
        var hasBucket = await BucketExistsAsync(bucketName);
        if (hasBucket) return true;

        PutBucketRequest request = new() { BucketName = bucketName, UseClientRegion = false };

        var response = await _s3Client.PutBucketAsync(request);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    private async Task<bool> BucketExistsAsync(string bucketName)
    {
        ListObjectsV2Request request = new()
        {
            BucketName = bucketName,
            MaxKeys = 1
        };

        try
        {
            var response = await _s3Client.ListObjectsV2Async(request);
            if (response == null) return false;

            if (response.HttpStatusCode != HttpStatusCode.OK) return false;

            return response.S3Objects.Count != 0;
        }
        catch (NoSuchBucketException e)
        {
            Console.WriteLine(e);
            logger.LogError(e, "Failed to check bucket exists");
        }

        return false;
    }
}