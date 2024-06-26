namespace SportHub.Services.Data.OlympicGamesDb.Interfaces;

using System.Linq.Expressions;

public interface IOlympicGamesRepositoryService<TEntity>
    where TEntity : class
{
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression);

    Task<TEntity> AddAsync(TEntity entity);

    TEntity Update(TEntity entity);
}