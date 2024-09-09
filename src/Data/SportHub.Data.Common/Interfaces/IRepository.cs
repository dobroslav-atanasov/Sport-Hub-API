﻿namespace SportHub.Data.Common.Interfaces;

using System.Linq.Expressions;

public interface IRepository<TEntity> : IDisposable
    where TEntity : class
{
    Task AddAsync(TEntity entity);

    Task AddRangeAsync(IEnumerable<TEntity> entities);

    IQueryable<TEntity> All();

    IQueryable<TEntity> AllAsNoTracking();

    Task<TEntity> GetAsync(params object[] id);

    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression);

    Task<int> SaveChangesAsync();

    void Update(TEntity entity);

    void Delete(TEntity entity);
}