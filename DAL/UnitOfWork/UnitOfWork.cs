using DAL.UnitOfWork.impl;
using DAL.Utils;
using Utils;

namespace DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    
    public UnitOfWork(ISettings settings)
    {
        Context = new DBContext(settings);
    }

    public DBContext Context { get; }

    public IAquariumRepository AquariumRepository => new AquariumRepository(Context);
    
    public IAquariumItemRepository AquariumItemRepository => new AquariumItemRepository(Context);
    
    public IUserRepository UserRepository => new UserRepository(Context, new PasswordHasher());

}