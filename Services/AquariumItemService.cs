using DAL;
using DAL.UnitOfWork;
using Services.Models;

namespace Services;

public abstract class AquariumItemService(IUnitOfWork unitOfWork, IRepository<AquariumItem> repository)
    : Service<AquariumItem>(unitOfWork, repository)
{
    public override Task<bool> Validate(AquariumItem entry)
    {
        if (string.IsNullOrEmpty(entry.Name))
        {
            ValidationDictionary.AddError("NameMissing", "Aquarium item name is required.");
        }

        return Task.FromResult(ValidationDictionary.IsValid);
    }

    public override async Task<ItemResponseModel<AquariumItem>> Create(AquariumItem entry)
    {
        ItemResponseModel<AquariumItem> response = new ItemResponseModel<AquariumItem>();

        if (await Validate(entry))
        {
            AquariumItem data = await Repository.InsertOneAsync(entry);
            response.Data = data;
        }
        else
        {
            response.ErrorMessages = ValidationDictionary.Errors;
        }

        return response;
    }

    public override async Task<ItemResponseModel<AquariumItem>> Update(string id, AquariumItem entry)
    {
        ItemResponseModel<AquariumItem> response = new ItemResponseModel<AquariumItem>();

        if (await Validate(entry))
        {
            AquariumItem existingItem = await Repository.FindByIdAsync(id);
            if (existingItem != null)
            {
                existingItem.Name = entry.Name;
                existingItem.Species = entry.Species;
                existingItem.Amount = entry.Amount;
                existingItem.Description = entry.Description;
                existingItem.Aquarium = entry.Aquarium;

                await Repository.InsertOrUpdateOneAsync(existingItem);
                response.Data = existingItem;
            }
            else
            {
                response.ErrorMessages.Add("NotFound", "Aquarium item not found.");
            }
        }
        else
        {
            response.ErrorMessages = ValidationDictionary.Errors;
        }

        return response;
    }

    protected async Task<ItemResponseModel<AquariumItem>> AddAquariumItem(AquariumItem entry)
    {
        ItemResponseModel<AquariumItem> response = new ItemResponseModel<AquariumItem>();

        if (await Validate(entry))
        {
            AquariumItem data = await Repository.InsertOneAsync(entry);
            response.Data = data;
        }
        else
        {
            response.ErrorMessages = ValidationDictionary.Errors;
        }

        return response;
    }
}