using DAL;
using DAL.UnitOfWork;
using Services.Models;

namespace Services;

public class CoralService : AquariumItemService
{
    public CoralService(IUnitOfWork unitOfWork, IRepository<AquariumItem> repository) 
        : base(unitOfWork, repository) { }

    public async Task<ItemResponseModel<AquariumItem>> AddCoral(Coral entry)
    {
        return await AddAquariumItem(entry);
    }

    public async Task<ItemResponseModel<Coral>> GetCoral(string id)
    {
        var coralItem = await Repository.FindByIdAsync(id) as Coral;

        if (coralItem == null)
        {
            return new ItemResponseModel<Coral>
            {
                Success = false,
                Message = $"Coral with ID {id} not found."
            };
        }

        return new ItemResponseModel<Coral>
        {
            Success = true,
            Data = coralItem
        };
    }
}
