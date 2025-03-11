using DAL;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Services.Models;
using Services.Utils;
using Utils;

namespace Services;

public abstract class Service<TEntitiy> : IService<TEntitiy> where TEntitiy : Entity
{

    protected Serilog.ILogger Log = AquariumLogger.Logger;
    protected IUnitOfWork UnitOfWork = null;
    protected IRepository<TEntitiy> Repository = null;
    protected IModelStateWrapper ValidationDictionary = null;
    protected ModelStateDictionary ModelState = null;
    protected User CurrentUser { get; private set; }
    
    
    public Service(IUnitOfWork unitOfWork, IRepository<TEntitiy> repository)
    {
        UnitOfWork = unitOfWork;
        Repository = repository;
    }
   
    public abstract Task<Boolean> Validate(TEntitiy entry);
    
    public virtual async Task<ItemResponseModel<TEntitiy>> CreateHandler(TEntitiy entitiy)
    {
        ValidationDictionary = new ModelStateWrapper(ModelState);
        if (await Validate(entitiy))
        {
            ItemResponseModel<TEntitiy> response = await Create(entitiy);
            return response;
        }
        else
        {
            ItemResponseModel<TEntitiy> response = new ItemResponseModel<TEntitiy>();
            response.Data = null;
            response.ErrorMessages = ValidationDictionary.Errors;
            return response;
        }
    }
    public virtual async Task<ItemResponseModel<TEntitiy>> UpdateHandler(String id, TEntitiy entitiy)
    {
        ValidationDictionary = new ModelStateWrapper(ModelState);
        if (await Validate(entitiy))
        {
            ItemResponseModel<TEntitiy> response = await Update(id, entitiy);
            return response;
        }
        else
        {
            ItemResponseModel<TEntitiy> response = new ItemResponseModel<TEntitiy>();
            response.Data = null;
            response.ErrorMessages = ValidationDictionary.Errors;
            return response;
        }
    }
    public abstract Task<ItemResponseModel<TEntitiy>> Create(TEntitiy entry);
    
    public abstract Task<ItemResponseModel<TEntitiy>> Update(string id, TEntitiy entry);
   
    public async Task<ActionResultResponseModel> Delete(string id)
    {
        await Repository.DeleteOneAsync(x => x.ID.Equals(id));

        ActionResultResponseModel responseModel = new ActionResultResponseModel();
        responseModel.Success = true;
        return responseModel;
    }

    public async Task SetModelState(ModelStateDictionary validation)
    {
        var validationDictionary = new ModelStateWrapper(validation);
        this.ValidationDictionary = validationDictionary;
    }
    
    public async Task Load(String email)
    {
        CurrentUser = await UnitOfWork.UserRepository.FindOneAsync(x => x.Email.ToLower().Equals(email.ToLower()));

        if (CurrentUser == null)
        {
            Log.Warning("Could not determine User");
        }
    }
    public Task<TEntitiy> Get(string id)
    {
        return Task.FromResult(Repository.FindByIdAsync(id).Result);
    }

    public Task<List<TEntitiy>> Get()
    {
        return Task.FromResult(Repository.FilterBy(x => true).ToList());
    }
}