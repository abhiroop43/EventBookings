using Carter;
using Documents.Api.Dtos;
using Mapster;
using MediatR;

namespace Documents.Api.Attachment.UploadAttachment;

public class UploadAttachmentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/upload", async (UploadRequest request, ISender sender) =>
            {
                var command = request.Adapt<UploadAttachmentCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<UploadResponse>();

                return Results.Created($"/download/{response.FileId}", response);
            })
            .WithName("Upload")
            .Produces<UploadResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Upload attachment")
            .WithDescription("Uploads an attachment and returns the key");
    }
}