using System.Linq.Expressions;

namespace DAL.UnitOfWork;

public interface IRepository<TEntity> where TEntity : Entity
{

    IEnumerable<TEntity> FilterBy(
        Expression<Func<TEntity, bool>> filterExpression);

    IEnumerable<TProjected> FilterBy<TProjected>(
        Expression<Func<TEntity, bool>> filterExpression,
        Expression<Func<TEntity, TProjected>> projectionExpression);

    Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression);
    
    Task<TEntity> FindByIdAsync(string id);

    Task<TEntity> InsertOneAsync(TEntity document);

    Task<TEntity> UpdateOneAsync(TEntity document);

    Task<TEntity> InsertOrUpdateOneAsync(TEntity document);

    Task DeleteOneAsync(Expression<Func<TEntity, bool>> filterExpression);
    
}