namespace Lookups.Api.Data;

public interface ILookupRepository
{
    Task<Models.Lookup?> GetDetailsAsync(string key, CancellationToken cancellationToken = default);
    Task<IEnumerable<Models.Lookup>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Models.Lookup>> GetByCategoryAsync(string lookupType,
        CancellationToken cancellationToken = default);

    Task<Models.Lookup> AddAsync(Models.Lookup lookup, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Models.Lookup lookup, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string key, CancellationToken cancellationToken = default);
}