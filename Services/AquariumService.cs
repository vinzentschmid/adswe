using DAL;
using DAL.UnitOfWork;
using Services.Models;

namespace Services;

public class AquariumService(IUnitOfWork unitOfWork, IRepository<Aquarium> repository)
    : Service<Aquarium>(unitOfWork, repository)
{
    public override Task<bool> Validate(Aquarium entry)
    {
        if (entry.Length <= 0)
        {
            ValidationDictionary.AddError("LengthMissing", "Aquarium length must be greater than 0");
        }
        if (entry.Depth <= 0)
        {
            ValidationDictionary.AddError("DetphMissing", "Aquarium depth must be greater than 0");
        }
        if (entry.Height <= 0)
        {
            ValidationDictionary.AddError("HeightMissing", "Aquarium height must be greater than 0");
        }
        

        return Task.FromResult(ValidationDictionary.IsValid);
    }

    public override async Task<ItemResponseModel<Aquarium>> Create(Aquarium entry)
    {
        ItemResponseModel<Aquarium> response = new ItemResponseModel<Aquarium>();
        entry.Liters = (entry.Depth * entry.Height * entry.Length) * 0.001;

        Aquarium data = await Repository.InsertOneAsync(entry);
        
        UserAquarium userAquarium = new UserAquarium();
        userAquarium.AquariumID = data.ID;
        userAquarium.UserID = CurrentUser.ID;
        
        await UnitOfWork.UserAquariumRepository.InsertOneAsync(userAquarium);

        response.Data = data;

        return response;

    }

    public override Task<ItemResponseModel<Aquarium>> Update(string id, Aquarium entry)
    {
        ItemResponseModel<Aquarium> response = new ItemResponseModel<Aquarium>();
        
        entry.Liters = (entry.Depth * entry.Height * entry.Length) * 0.001;

        response.Data = entry;

        return Task.FromResult(response);
    }
}