using Carter;
using Lookups.Api.Dtos;
using Mapster;
using MediatR;

namespace Lookups.Api.Lookup.AddLookup;

public record AddLookupRequest(AddLookupDto Lookup);

public record AddLookupResponse(string Id);

public class AddLookupEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/lookup",
                async (AddLookupRequest request, ISender sender) =>
                {
                    var command = request.Adapt<AddLookupCommand>();
                    var result = await sender.Send(command);
                    var response = result.Adapt<AddLookupResponse>();

                    return Results.Created($"/lookup/{response.Id}", response);
                }
            )
            .WithName("AddLookup")
            .Produces<AddLookupResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Add lookup")
            .WithDescription("Adds a new lookup");
    }
}
