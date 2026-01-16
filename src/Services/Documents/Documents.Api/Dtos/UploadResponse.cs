namespace Documents.Api.Dtos;

public record UploadResponse(bool IsSuccess, string FileId, string? ErrorMessage = null);