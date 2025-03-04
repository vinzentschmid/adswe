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
        return _collection.Find(filterExpression).ToEnumerable();
    }

    public IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TProjected>> projectionExpression)
    {
        return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
    }

    public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression)
    {
        return await _collection.Find(filterExpression).FirstOrDefaultAsync();
    }

    public async Task<TEntity> FindByIdAsync(string id)
    {
        var filter = Builders<TEntity>.Filter.Eq(doc => doc.ID, id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<TEntity> InsertOneAsync(TEntity document)
    {
        document.ID = document.GenerateNewID();
        await _collection.InsertOneAsync(document);
        return document;
    }

    public async Task<TEntity> UpdateOneAsync(TEntity document)
    {
        var filter = Builders<TEntity>.Filter.Eq(doc => doc.ID, document.ID);
        await _collection.ReplaceOneAsync(filter, document);
        return document;
    }

    public async Task<TEntity> InsertOrUpdateOneAsync(TEntity document)
    {
        var filter = Builders<TEntity>.Filter.Eq(doc => doc.ID, document.ID);
        var existingDocument = await _collection.Find(filter).FirstOrDefaultAsync();

        if (existingDocument == null)
        {
            document.ID = document.GenerateNewID();
            await _collection.InsertOneAsync(document);
        }
        else
        {
            await _collection.ReplaceOneAsync(filter, document);
        }

        return document;
    }

    public async Task DeleteOneAsync(Expression<Func<TEntity, bool>> filterExpression)
    {
        await _collection.DeleteOneAsync(filterExpression);
    }
}