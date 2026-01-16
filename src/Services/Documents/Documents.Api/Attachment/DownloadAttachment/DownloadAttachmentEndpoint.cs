namespace Documents.Api.Attachment.DownloadAttachment;

public record DownloadResponse(string FileContent);

public class DownloadAttachmentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/download/{fileId}", async (string fileId, ISender sender) =>
            {
                var query = new DownloadAttachmentQuery(fileId);
                var result = await sender.Send(query);
                return result.Adapt<DownloadResponse>();
            })
            .WithName("Download")
            .Produces<DownloadResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Download Attachment")
            .WithDescription("Downloads an attachment with the given fileId");
    }
}