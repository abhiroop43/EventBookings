namespace Documents.Api.Dtos;

public record UploadRequest(string FileName, string FileContent, string ContentType);