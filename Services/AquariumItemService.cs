using DAL;
using DAL.UnitOfWork;
using Services.Models;

namespace Services;

public abstract class AquariumItemService(IUnitOfWork unitOfWork, IRepository<AquariumItem> repository)
    : Service<AquariumItem>(unitOfWork, repository)
{
    public override Task<bool> Validate(AquariumItem entry)
    {
        if (entry.Name == "")
        {
            throw new ArgumentNullException(nameof(entry), "Name is required.");
        }

        return Task.FromResult(ValidationDictionary.IsValid);
    }

    public override async Task<ItemResponseModel<AquariumItem>> Create(AquariumItem entry)
    {
        return await AddAquariumItem(entry);
    }

    public override async Task<ItemResponseModel<AquariumItem>> Update(string id, AquariumItem entry)
    {

        var existingItem = await Repository.FindByIdAsync(id);
        if (existingItem == null)
        {
            return new ItemResponseModel<AquariumItem>
            {
                Success = false,
                ErrorMessages = { { "NotFound", "Aquarium item not found." } }
            };
        }

        // Werte aktualisieren
        existingItem.Name = entry.Name;
        existingItem.Species = entry.Species;
        existingItem.Amount = entry.Amount;
        existingItem.Description = entry.Description;
        existingItem.Aquarium = entry.Aquarium;

        await Repository.UpdateOneAsync(existingItem);

        return new ItemResponseModel<AquariumItem> { Data = existingItem };
    }

    protected async Task<ItemResponseModel<AquariumItem>> AddAquariumItem(AquariumItem entry)
    {
        if (Repository == null)
        {
            throw new InvalidOperationException("Repository is not initialized.");
        }

        var data = await Repository.InsertOneAsync(entry);
        
        if (data == null)
        {
            throw new InvalidOperationException("Failed to insert the aquarium item.");
        }

        return new ItemResponseModel<AquariumItem>
        {
            Data = data,
            Success = true
        };
    }
}
