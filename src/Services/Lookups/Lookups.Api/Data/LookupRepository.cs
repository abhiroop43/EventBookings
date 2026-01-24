using Lookups.Api.Data.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Lookups.Api.Data;

public class LookupRepository(LookupsDbContext dbContext) : ILookupRepository
{
    public async Task<Models.Lookup?> GetDetailsAsync(
        string key,
        CancellationToken cancellationToken = default
    )
    {
        return await dbContext
            .Lookups.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Key == key, cancellationToken);
    }

    public async Task<IEnumerable<Models.Lookup>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await dbContext.Lookups.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Models.Lookup>> GetByCategoryAsync(
        string lookupType,
        CancellationToken cancellationToken = default
    )
    {
        return await dbContext
            .Lookups.AsNoTracking()
            .Where(x => x.Type == lookupType)
            .ToListAsync(cancellationToken);
    }

    public async Task<Models.Lookup> AddAsync(
        Models.Lookup lookup,
        CancellationToken cancellationToken = default
    )
    {
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
            x => x.Key == lookup.Key,
            cancellationToken
        );

        if (existingLookup is null)
        {
            return false;
        }

        existingLookup.Value = lookup.Value;
        existingLookup.Key = lookup.Key;
        existingLookup.Type = lookup.Type;
        existingLookup.Children = lookup.Children;

        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        var existingLookup = await dbContext.Lookups.FirstOrDefaultAsync(
            x => x.Key == key,
            cancellationToken
        );

        if (existingLookup is null)
        {
            return false;
        }

        dbContext.Lookups.Remove(existingLookup);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}