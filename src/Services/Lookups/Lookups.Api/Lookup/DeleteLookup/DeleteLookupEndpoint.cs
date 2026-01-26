using Carter;
using MediatR;
using MongoDB.Bson;

namespace Lookups.Api.Lookup.DeleteLookup;

public class DeleteLookupEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(
                "/lookup/{id}",
                async (ObjectId id, ISender sender) =>
                {
                    await sender.Send(new DeleteLookupCommand(id));
                    return Results.NoContent();
                }
            )
            .WithName("DeleteLookup")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete a Lookup")
            .WithDescription("Delete an existing lookup by id");
    }
}
