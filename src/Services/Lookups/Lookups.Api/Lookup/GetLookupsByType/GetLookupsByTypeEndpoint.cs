using BuildingBlocks.Pagination;
using Carter;
using Lookups.Api.Dtos;
using Mapster;
using MediatR;

namespace Lookups.Api.Lookup.GetLookupsByType;

public record GetLookupsByTypeRequest(string CategoryType, int PageNumber = 1, int PageSize = 10);

public record GetLookupsByTypeResponse(PaginatedResult<GetLookupsDto> Lookups);

public class GetLookupsByTypeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/lookup/{categoryType}",
                async (
                    string categoryType,
                    [AsParameters] GetLookupsByTypeRequest request,
                    ISender sender
                ) =>
                {
                    var result = await sender.Send(
                        new GetLookupByTypesQuery(
                            request.CategoryType,
                            request.PageNumber,
                            request.PageSize
                        )
                    );

                    return Results.Ok(result.Adapt<GetLookupsByTypeResponse>());
                }
            )
            .WithName("GetLookupsByType")
            .Produces<GetLookupsByTypeResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Lookups By Type")
            .WithDescription("Gets all lookups by category type");
    }
}
