using DAL;
using DAL.UnitOfWork;
using Services.Models;

namespace Services;

public class CoralService(IUnitOfWork unitOfWork, IRepository<AquariumItem> repository) : AquariumItemService(unitOfWork, repository)
{
    public async Task<ItemResponseModel<AquariumItem>> AddCoral(Coral entry)
    {
        var response = await AddAquariumItem(entry);
        return response;
    }
    public async Task<ItemResponseModel<Coral>> GetCoral(string id)
    {
        var coralItem = await Repository.FindByIdAsync(id);
        

        return new ItemResponseModel<Coral> { Data = (Coral)coralItem };
    }
}