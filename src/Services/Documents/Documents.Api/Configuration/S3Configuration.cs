namespace Documents.Api.Configuration;

public sealed class S3Configuration
{
    public const string Section = "RustFS";

    /// <summary>
    ///     the URL where RustFS is hosted
    /// </summary>
    public string ServiceUrl { get; set; } = string.Empty;

    /// <summary>
    ///     The access key to authenticate with RustFS
    /// </summary>
    public string AccessKey { get; set; } = string.Empty;

    /// <summary>
    ///     the secret key to authenticate with RustFS
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    ///     since this is locally hosted, can be set to us-east-1
    /// </summary>
    public string Region { get; set; } = string.Empty;

    /// <summary>
    ///     the bucket name configured for this application
    /// </summary>
    public string Bucket { get; set; } = string.Empty;
}