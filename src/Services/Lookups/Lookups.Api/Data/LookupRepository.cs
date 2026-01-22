using Lookups.Api.Data.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Lookups.Api.Data;

public class LookupRepository(LookupsDbContext dbContext) : ILookupRepository
{
    public async Task<Models.Lookup?> GetDetailsAsync(string key, CancellationToken cancellationToken = default)
    {
        return await dbContext.Lookups.FirstOrDefaultAsync(x => x.Key == key, cancellationToken);
    }

    public async Task<IEnumerable<Models.Lookup>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Lookups.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Models.Lookup>> GetByCategoryAsync(string category,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Lookups.Where(x => x.Type == category).ToListAsync(cancellationToken);
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