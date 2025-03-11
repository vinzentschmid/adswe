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

        // Save the Aquarium object to the database
        var data = await Repository.InsertOneAsync(entry);

        // Check if the Aquarium object was successfully saved
        if (data != null)
        {
            // Create the UserAquarium object
            var userAquarium = new UserAquarium
            {
                AquariumID = data.ID,
                UserID = CurrentUser.ID
            };

            // Save the UserAquarium object to the database
            await UnitOfWork.UserAquariumRepository.InsertOneAsync(userAquarium);

            response.Data = data;
        }
        else
        {
            response.Success = false;
            response.Message = "Failed to create Aquarium.";
        }

        return response;
    }

    public override async Task<ItemResponseModel<Aquarium>> Update(string id, Aquarium entry)
    {
        ItemResponseModel<Aquarium> response = new ItemResponseModel<Aquarium>();

        // Fetch the existing Aquarium object from the database
        var existingAquarium = await Repository.FindByIdAsync(id);
        if (existingAquarium == null)
        {
            response.Success = false;
            response.Message = "Aquarium not found.";
            return response;
        }

        // Update the existing Aquarium object with new values
        existingAquarium.Name = entry.Name;
        existingAquarium.Depth = entry.Depth;
        existingAquarium.Height = entry.Height;
        existingAquarium.Length = entry.Length;
        existingAquarium.WaterType = entry.WaterType;
        existingAquarium.Liters = (entry.Depth * entry.Height * entry.Length) * 0.001;

        // Save the updated Aquarium object to the database
        var updatedAquarium = await Repository.UpdateOneAsync(existingAquarium);

        response.Data = updatedAquarium;
        return response;
    }
    
    public async Task<List<Aquarium>> GetAllAquariumsForUser(string username)
    {
        // Fetch the user by username
        var user = await UnitOfWork.UserRepository.FindByUsernameAsync(username);
        if (user == null)
        {
            return new List<Aquarium>();
        }

        // Fetch all UserAquarium objects for the specified user
        var userAquariums =  UnitOfWork.UserAquariumRepository.FilterBy(ua => ua.UserID == user.ID).ToList();

        // Extract the Aquarium IDs from the UserAquarium objects
        var aquariumIds = userAquariums.Select(ua => ua.AquariumID).ToList();

        // Fetch the corresponding Aquarium objects
        var aquariums =  UnitOfWork.AquariumRepository.FilterBy(a => aquariumIds.Contains(a.ID)).ToList();

        return aquariums;
    }
}