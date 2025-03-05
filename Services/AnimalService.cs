using DAL;
using DAL.UnitOfWork;
using Services.Models;

namespace Services;

public class AnimalService(IUnitOfWork unitOfWork, IRepository<AquariumItem> repository) : AquariumItemService(unitOfWork, repository)
{
    public async Task<ItemResponseModel<AquariumItem>> AddAnimal(Animal entry)
    {
        var response = await AddAquariumItem(entry);
        return response;
    }
    public async Task<ItemResponseModel<Animal>> GetAnimal(string id)
    {
        var animalItem = await Repository.FindByIdAsync(id);
        if (animalItem == null)
        {
            return new ItemResponseModel<Animal>
            {
                ErrorMessages = new Dictionary<string, string> { { "NotFound", "Animal not found." } }
            };
        }

        return new ItemResponseModel<Animal> { Data = (Animal)animalItem };
    }
}