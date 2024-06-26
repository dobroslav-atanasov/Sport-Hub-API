namespace SportHub.Data.Repositories;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using SportHub.Data.Common.Interfaces;
using SportHub.Data.Contexts;

public class OlympicGamesRepository<TEntity> : IRepository<TEntity>, IDeletableEntityRepository<TEntity>
    where TEntity : class, IDeletableEntity
{
    public OlympicGamesRepository(OlympicGamesDbContext context)
    {
        this.Context = context;
        this.DbSet = this.Context.Set<TEntity>();
    }

    protected OlympicGamesDbContext Context { get; }

    protected DbSet<TEntity> DbSet { get; }

    public async Task AddAsync(TEntity entity)
    {
        await this.DbSet.AddAsync(entity).AsTask();
    }

    public IQueryable<TEntity> All()
    {
        return this.DbSet.Where(x => !x.IsDeleted);
    }

    public IQueryable<TEntity> AllAsNoTracking()
    {
        return this.DbSet.Where(x => !x.IsDeleted).AsNoTracking();
    }

    public IQueryable<TEntity> AllWithDeleted()
    {
        return this.DbSet.IgnoreQueryFilters();
    }

    public IQueryable<TEntity> AllAsNoTrackingWithDeleted()
    {
        return this.DbSet.AsNoTracking().IgnoreQueryFilters();
    }

    public void Delete(TEntity entity)
    {
        entity.IsDeleted = true;
        entity.DeletedOn = DateTime.UtcNow;
        this.Update(entity);
    }

    public async Task<TEntity> GetAsync(params object[] id)
    {
        return await this.DbSet.FindAsync(id);
    }

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await this.DbSet.Where(expression).FirstOrDefaultAsync();
    }

    public void HardDelete(TEntity entity)
    {
        this.DbSet.Remove(entity);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await this.Context.SaveChangesAsync();
    }

    public void Undelete(TEntity entity)
    {
        entity.IsDeleted = false;
        entity.DeletedOn = null;
        this.Update(entity);
    }

    public void Update(TEntity entity)
    {
        var entry = this.Context.Entry(entity);
        if (entry.State == EntityState.Detached)
        {
            this.DbSet.Attach(entity);
        }

        entry.State = EntityState.Modified;
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.Context?.Dispose();
        }
    }
}