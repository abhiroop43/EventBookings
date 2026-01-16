using System.Net;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using BuildingBlocks.CQRS;
using Documents.Api.Configuration;
using Documents.Api.Dtos;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Documents.Api.Attachment.UploadAttachment;

public record UploadAttachmentCommand(UploadRequest Request) : ICommand<UploadAttachmentResult>;

public record UploadAttachmentResult(string FileId);

public class UploadAttachmentCommandValidator : AbstractValidator<UploadAttachmentCommand>
{
    public UploadAttachmentCommandValidator()
    {
        RuleFor(x => x.Request).NotNull();
        RuleFor(x => x.Request.FileName).NotEmpty().WithMessage("File Name is required");
        RuleFor(x => x.Request.FileContent).NotEmpty().WithMessage("File Content is required");
    }
}

public class UploadAttachmentCommandHandler(
    ILogger<UploadAttachmentCommandHandler> logger,
    IOptionsSnapshot<S3Configuration> configuration) : ICommandHandler<UploadAttachmentCommand, UploadAttachmentResult>
{
    private IAmazonS3 _s3Client;

    public async Task<UploadAttachmentResult> Handle(UploadAttachmentCommand request,
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
        if (!await BucketExistsAsync(configuration.Value.Bucket))
        {
            var bucketCreated = await CreateBucketAsync(configuration.Value.Bucket);

            if (!bucketCreated) return new UploadAttachmentResult(string.Empty);
        }

        PutObjectRequest objectRequest = new()
        {
            BucketName = configuration.Value.Bucket,
            Key = request.Request.FileName,
            InputStream = new MemoryStream(Convert.FromBase64String(request.Request.FileContent)),
            ContentType = request.Request.ContentType
        };

        var response = await _s3Client.PutObjectAsync(objectRequest, cancellationToken);

        if (response.HttpStatusCode != HttpStatusCode.OK)
            throw new Exception($"Failed to upload attachment to S3. Status code: {response.HttpStatusCode}");

        return new UploadAttachmentResult(request.Request.FileName);
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

        var response = await _s3Client.ListObjectsV2Async(request);

        if (response.HttpStatusCode != HttpStatusCode.OK) return false;

        return response.S3Objects.Count != 0;
    }
}