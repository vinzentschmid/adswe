using DAL.UnitOfWork;
using Services;

namespace AquariumAPI;

public interface IGlobalService
{
    public AquariumService AquariumService { get; set; }
    public UserService UserService { get; set; }
    public CoralService CoralService { get; set; }

    //public PictureService PictureService { get; set; }
    public AnimalService AnimalService { get; set; }
    public IUnitOfWork UnitOfWork { get; set; }
}