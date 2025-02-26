using DAL.UnitOfWork.impl;
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
}