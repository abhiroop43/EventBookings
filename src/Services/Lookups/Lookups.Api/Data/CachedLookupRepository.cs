using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;

namespace Lookups.Api.Data;

public class CachedLookupRepository(ILookupRepository repository, IDistributedCache cache)
    : ILookupRepository
{
    public async Task<Models.Lookup?> GetDetailsAsync(
        ObjectId id,
        CancellationToken cancellationToken = default
    )
    {
        var cachedLookupDetails = await cache.GetStringAsync(id.ToString(), cancellationToken);

        if (!string.IsNullOrEmpty(cachedLookupDetails))
        {
            var deserializeLookupDetails = JsonSerializer.Deserialize<Models.Lookup>(
                cachedLookupDetails
            );
            if (deserializeLookupDetails != null)
                return deserializeLookupDetails;
        }

        var lookupDetails = await repository.GetDetailsAsync(id, cancellationToken);
        await cache.SetStringAsync(
            id.ToString(),
            JsonSerializer.Serialize(lookupDetails),
            cancellationToken
        );
        return lookupDetails;
    }

    public async Task<IList<Models.Lookup>> GetByCategoryAsync(
        string lookupType,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default
    )
    {
        var cacheKey = $"{lookupType}_{pageNumber}_{pageSize}";
        var cachedLookups = await cache.GetStringAsync(cacheKey, cancellationToken);

        if (!string.IsNullOrEmpty(cachedLookups))
        {
            var deserializedLookups = JsonSerializer.Deserialize<IList<Models.Lookup>>(
                cachedLookups
            );
            if (deserializedLookups != null)
                return deserializedLookups;
        }

        var lookups = await repository.GetByCategoryAsync(
            lookupType,
            pageNumber,
            pageSize,
            cancellationToken
        );
        await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(lookups), cancellationToken);
        return lookups;
    }

    public async Task<Models.Lookup> AddAsync(
        Models.Lookup lookup,
        CancellationToken cancellationToken = default
    )
    {
        var savedLookup = await repository.AddAsync(lookup, cancellationToken);
        await cache.SetStringAsync(
            lookup.Key,
            JsonSerializer.Serialize(savedLookup),
            cancellationToken
        );
        return savedLookup;
    }

    public async Task<bool> UpdateAsync(
        Models.Lookup lookup,
        CancellationToken cancellationToken = default
    )
    {
        var updated = await repository.UpdateAsync(lookup, cancellationToken);
        if (!updated)
            return false;
        await cache.SetStringAsync(lookup.Key, JsonSerializer.Serialize(lookup), cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var deleted = await repository.DeleteAsync(id, cancellationToken);
        if (!deleted)
            return false;
        await cache.RemoveAsync(id.ToString(), cancellationToken);
        return true;
    }
}
