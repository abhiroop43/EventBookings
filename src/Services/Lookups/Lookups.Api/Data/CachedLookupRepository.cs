using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Lookups.Api.Data;

public class CachedLookupRepository(ILookupRepository repository, IDistributedCache cache)
    : ILookupRepository
{
    public async Task<Models.Lookup?> GetDetailsAsync(
        string key,
        CancellationToken cancellationToken = default
    )
    {
        var cachedLookupDetails = await cache.GetStringAsync(key, cancellationToken);

        if (!string.IsNullOrEmpty(cachedLookupDetails))
        {
            var deserializeLookupDetails = JsonSerializer.Deserialize<Models.Lookup>(
                cachedLookupDetails
            );
            if (deserializeLookupDetails != null)
            {
                return deserializeLookupDetails;
            }
        }

        var lookupDetails = await repository.GetDetailsAsync(key, cancellationToken);
        await cache.SetStringAsync(key, JsonSerializer.Serialize(lookupDetails), cancellationToken);
        return lookupDetails;
    }

    public Task<IEnumerable<Models.Lookup>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Models.Lookup>> GetByCategoryAsync(
        string lookupType,
        CancellationToken cancellationToken = default
    )
    {
        throw new NotImplementedException();
    }

    public Task<Models.Lookup> AddAsync(
        Models.Lookup lookup,
        CancellationToken cancellationToken = default
    )
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(
        Models.Lookup lookup,
        CancellationToken cancellationToken = default
    )
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}