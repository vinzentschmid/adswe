using DAL.UnitOfWork.impl;

namespace DAL.UnitOfWork;

public interface IUnitOfWork
{
    DBContext Context { get; }
    
    IAquariumRepository AquariumRepository { get; }
    
    IUserRepository UserRepository { get; }
    
    IAquariumItemRepository AquariumItemRepository { get; }
    
    IRepository<UserAquarium> UserAquariumRepository { get; }

}