using Carter;
using Lookups.Api.Dtos;
using Mapster;
using MediatR;

namespace Lookups.Api.Lookup.UpdateLookup;

public record UpdateLookupRequest(UpdateLookupDto Lookup);

public record UpdateLookupResponse(bool IsUpdated);

public class UpdateLookupEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut(
                "/lookup",
                async (UpdateLookupRequest request, ISender sender) =>
                {
                    var command = request.Adapt<UpdateLookupCommand>();
                    var result = await sender.Send(command);
                    return Results.Ok(result.Adapt<UpdateLookupResponse>());
                }
            )
            .WithName("UpdateLookup")
            .Produces<UpdateLookupResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Lookup")
            .WithDescription("Updates an existing lookup");
    }
}
