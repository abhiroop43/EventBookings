using MongoDB.Bson;

namespace Lookups.Api.Data;

public interface ILookupRepository
{
    Task<Models.Lookup?> GetDetailsAsync(
        ObjectId id,
        CancellationToken cancellationToken = default
    );

    Task<IList<Models.Lookup>> GetByCategoryAsync(
        string lookupType,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default
    );

    Task<Models.Lookup> AddAsync(
        Models.Lookup lookup,
        CancellationToken cancellationToken = default
    );

    Task<bool> UpdateAsync(Models.Lookup lookup, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(ObjectId id, CancellationToken cancellationToken = default);
}
