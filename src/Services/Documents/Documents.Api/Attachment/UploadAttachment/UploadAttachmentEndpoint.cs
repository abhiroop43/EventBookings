namespace Documents.Api.Attachment.UploadAttachment;

public record UploadRequest(string FileName, string FileContent, string ContentType);

public record UploadResponse(bool IsSuccess, string FileId, string? ErrorMessage = null);

public class UploadAttachmentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/upload",
                async (UploadRequest request, ISender sender) =>
                {
                    var command = request.Adapt<UploadAttachmentCommand>();
                    var result = await sender.Send(command);
                    var response = result.Adapt<UploadResponse>();

                    return Results.Created($"/download/{response.FileId}", response);
                }
            )
            .WithName("Upload")
            .Produces<UploadResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Upload Attachment")
            .WithDescription("Uploads an attachment and returns the key");
    }
}