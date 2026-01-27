using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using Lookups.Api.Data;

namespace Lookups.Api.Lookup.GetLookupsByType;

public record GetLookupByTypesQuery(string LookupType, int PageNumber = 1, int PageSize = 10)
    : PaginationRequest,
        IQuery<GetLookupByTypesResult>;

public record GetLookupByTypesResult(PaginatedResult<Models.Lookup> Lookups);

public class GetLookupByTypesQueryHandler(ILookupRepository repository)
    : IQueryHandler<GetLookupByTypesQuery, GetLookupByTypesResult>
{
    public async Task<GetLookupByTypesResult> Handle(
        GetLookupByTypesQuery query,
        CancellationToken cancellationToken
    )
    {
        var lookups = await repository.GetByCategoryAsync(
            query.LookupType,
            query.PageNumber,
            query.PageSize,
            cancellationToken
        );
        return new GetLookupByTypesResult(
            new PaginatedResult<Models.Lookup>(
                query.PageNumber,
                query.PageSize,
                lookups.Count,
                lookups
            )
        );
    }
}
