using DAL.UnitOfWork;
using Services;
using Services.Authentication;

namespace AquariumAPI;

public class GlobalService : IGlobalService
{
    public AquariumService AquariumService { get; set; }
    public UserService UserService { get; set; }
    public CoralService CoralService { get; set; }
    public AnimalService AnimalService { get; set; }
    public IUnitOfWork UnitOfWork { get; set; }

    public GlobalService(IUnitOfWork unitOfWork, IAuthentication authentication)
    {
        UnitOfWork = unitOfWork;
        AquariumService = new AquariumService(unitOfWork, unitOfWork.AquariumRepository);
        UserService = new UserService(unitOfWork, unitOfWork.UserRepository, authentication);
        CoralService = new CoralService(unitOfWork, unitOfWork.AquariumItemRepository);
        AnimalService = new AnimalService(unitOfWork, unitOfWork.AquariumItemRepository);
    }
}