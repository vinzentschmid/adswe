using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL.UnitOfWork;

public class Repository<TEntity>(DBContext context) : IRepository<TEntity>
    where TEntity : Entity
{
    private IMongoCollection<TEntity> _collection = context.DataBase.GetCollection<TEntity>(typeof(TEntity).Name);

    public IEnumerable<TEntity> FilterBy(Expression<Func<TEntity, bool>> filterExpression)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TProjected>> projectionExpression)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> FindByIdAsync(string id)
    {
        var filter = Builders<TEntity>.Filter.Eq(doc => doc.ID, id);
        return _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<TEntity> InsertOneAsync(TEntity document)
    {
        document.ID = document.GenerateNewID();
        await _collection.InsertOneAsync(document);
        return document;
    }

    public Task<TEntity> UpdateOneAsync(TEntity document)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> InsertOrUpdateOneAsync(TEntity document)
    {
        throw new NotImplementedException();
    }

    public Task DeleteOneAsync(Expression<Func<TEntity, bool>> filterExpression)
    {
        throw new NotImplementedException();
    }
}