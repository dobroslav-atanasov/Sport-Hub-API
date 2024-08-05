namespace SportHub.Data.Contexts;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using SportHub.Data.Common.Interfaces;
using SportHub.Data.Models.DbEntities.SportHub;

public class SportHubDbContext : IdentityDbContext<User, Role, string>
{
    public SportHubDbContext(DbContextOptions<SportHubDbContext> options)
        : base(options)
    {
    }

    public override int SaveChanges()
    {
        return this.SaveChanges(true);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.ApplyCheckRules();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return this.SaveChangesAsync(true, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        this.ApplyCheckRules();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void ApplyCheckRules()
    {
        var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e => e.Entity is ICreatableEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in changedEntries)
        {
            var entity = (ICreatableEntity)entry.Entity;
            if (entry.State == EntityState.Added && entity.CreatedOn == default)
            {
                entity.CreatedOn = DateTime.UtcNow;
            }
            else
            {
                entity.ModifiedOn = DateTime.UtcNow;
            }
        }
    }
}