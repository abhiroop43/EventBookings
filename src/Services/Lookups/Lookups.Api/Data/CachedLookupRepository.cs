using Microsoft.Extensions.Caching.Distributed;

namespace Lookups.Api.Data;

public class CachedLookupRepository(IDistributedCache cache) : ILookupRepository
{
    public Task<Models.Lookup?> GetDetailsAsync(string key, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Models.Lookup>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Models.Lookup>> GetByCategoryAsync(string category,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Models.Lookup> AddAsync(Models.Lookup lookup, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Models.Lookup lookup, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}