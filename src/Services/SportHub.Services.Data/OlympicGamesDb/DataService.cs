﻿namespace SportHub.Services.Data.OlympicGamesDb;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using SportHub.Data.Common.Interfaces;
using SportHub.Data.Factories.Interfaces;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;

public class DataService<TEntity> : IDataService<TEntity>
    where TEntity : class, IDeletableEntity
{
    private readonly IDbContextFactory dbContextFactory;

    public DataService(IDbContextFactory dbContextFactory)
    {
        this.dbContextFactory = dbContextFactory;
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        using var context = this.dbContextFactory.CreateOlympicGamesDbContext();

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        return entity;
    }

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
    {
        using var context = this.dbContextFactory.CreateOlympicGamesDbContext();

        var dbSet = context.Set<TEntity>();
        var entity = await dbSet
            .Where(expression)
            .FirstOrDefaultAsync();

        return entity;
    }

    public TEntity Update(TEntity entity)
    {
        using var context = this.dbContextFactory.CreateOlympicGamesDbContext();

        var entry = context.Entry(entity);
        if (entry.State == EntityState.Detached)
        {
            context.Attach(entity);
        }

        entry.State = EntityState.Modified;

        context.SaveChanges();

        return entity;
    }
}