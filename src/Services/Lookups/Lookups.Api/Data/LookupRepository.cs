using Lookups.Api.Data.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace Lookups.Api.Data;

public class LookupRepository(LookupsDbContext dbContext) : ILookupRepository
{
    public async Task<Models.Lookup?> GetDetailsAsync(
        ObjectId id,
        CancellationToken cancellationToken = default
    )
    {
        return await dbContext
            .Lookups.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IList<Models.Lookup>> GetByCategoryAsync(
        string lookupType,
        CancellationToken cancellationToken = default
    )
    {
        return await dbContext
            .Lookups.AsNoTracking()
            .Where(x => x.LookupType == lookupType)
            .ToListAsync(cancellationToken);
    }

    public async Task<Models.Lookup> AddAsync(
        Models.Lookup lookup,
        CancellationToken cancellationToken = default
    )
    {
        lookup.Id = ObjectId.GenerateNewId();
        await dbContext.Lookups.AddAsync(lookup, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return lookup;
    }

    public async Task<bool> UpdateAsync(
        Models.Lookup lookup,
        CancellationToken cancellationToken = default
    )
    {
        var existingLookup = await dbContext.Lookups.FirstOrDefaultAsync(
            x => x.Id == lookup.Id,
            cancellationToken
        );

        if (existingLookup is null)
            return false;

        existingLookup.Value = lookup.Value;
        existingLookup.Key = lookup.Key;
        existingLookup.LookupType = lookup.LookupType;
        existingLookup.Children = lookup.Children;

        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var existingLookup = await dbContext.Lookups.FirstOrDefaultAsync(
            x => x.Id == id,
            cancellationToken
        );

        if (existingLookup is null)
            return false;

        dbContext.Lookups.Remove(existingLookup);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
