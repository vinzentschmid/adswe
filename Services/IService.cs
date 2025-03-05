using DAL;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Services.Models;

namespace Services;

public interface IService<TEntity> where TEntity : Entity
{
    public abstract Task<ItemResponseModel<TEntity>> Create(TEntity entry);
    public abstract Task<ItemResponseModel<TEntity>> Update(string id, TEntity entry);
    public Task<ActionResultResponseModel> Delete(String id);
    public Task SetModelState(ModelStateDictionary validation);
    public Task Load(String email);
    public Task<TEntity> Get(String id);
    public Task<List<TEntity>> Get();
}