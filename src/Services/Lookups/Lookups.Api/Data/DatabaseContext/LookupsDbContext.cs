using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Lookups.Api.Data.DatabaseContext;

public class LookupsDbContext(DbContextOptions<LookupsDbContext> options) : DbContext(options)
{
    public DbSet<Models.Lookup> Lookups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LookupsDbContext).Assembly);
        modelBuilder.Entity<Models.Lookup>().ToCollection("lookups");
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in base.ChangeTracker.Entries<Models.Lookup>())
        {
            entry.Entity.UpdatedAt = DateTime.UtcNow;
            entry.Entity.UpdatedBy = "System"; // will be updated later to the current user id

            if (entry.State != EntityState.Added) continue;

            entry.Entity.CreatedAt = DateTime.UtcNow;
            entry.Entity.CreatedBy = "System"; // will be updated later to the current user id
        }

        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
        return base.SaveChangesAsync(cancellationToken);
    }
}