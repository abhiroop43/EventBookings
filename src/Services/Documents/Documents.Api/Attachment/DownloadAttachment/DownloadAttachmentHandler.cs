namespace Documents.Api.Attachment.DownloadAttachment;

public record DownloadAttachmentQuery(string FileId) : IQuery<DownloadAttachmentResult>;

public record DownloadAttachmentResult(string? FileContent);

public class DownloadAttachmentQueryHandler(
    ILogger<DownloadAttachmentQueryHandler> logger,
    IOptionsSnapshot<S3Configuration> configuration) : IQueryHandler<DownloadAttachmentQuery, DownloadAttachmentResult>
{
    public async Task<DownloadAttachmentResult> Handle(DownloadAttachmentQuery query,
        CancellationToken cancellationToken)
    {
        var clientConfig = new AmazonS3Config
        {
            ServiceURL = configuration.Value.ServiceUrl,
            ForcePathStyle = true,
            UseHttp = true
        };

        var s3Client = new AmazonS3Client(
            new BasicAWSCredentials(configuration.Value.AccessKey, configuration.Value.SecretKey), clientConfig);
        GetObjectRequest request = new()
        {
            BucketName = configuration.Value.Bucket,
            Key = query.FileId
        };

        var response = await s3Client.GetObjectAsync(request, cancellationToken);

        if (response.HttpStatusCode != HttpStatusCode.OK)
        {
            logger.LogError("Failed to download file, Status Code: {StatusCode}", response.HttpStatusCode);
            return new DownloadAttachmentResult(null);
        }

        using var memoryStream = new MemoryStream();
        await response.ResponseStream.CopyToAsync(memoryStream, cancellationToken);
        var byteArray = memoryStream.ToArray();

        return new DownloadAttachmentResult(Convert.ToBase64String(byteArray));
    }
}