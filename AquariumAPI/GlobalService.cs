using DAL.UnitOfWork;
using Services;
using Services.Authentication;

namespace AquariumAPI;

public class GlobalService(IUnitOfWork unitOfWork, IAuthentication authentication) : IGlobalService
{
    public AquariumService AquariumService { get; set; } = new(unitOfWork, unitOfWork.AquariumRepository);
    public UserService UserService { get; set; } = new(unitOfWork, unitOfWork.UserRepository, authentication);
    public CoralService CoralService { get; set; } = new(unitOfWork, unitOfWork.AquariumItemRepository);
    public AnimalService AnimalService { get; set; } = new(unitOfWork, unitOfWork.AquariumItemRepository);
    public IUnitOfWork UnitOfWork { get; set; } = unitOfWork;
}