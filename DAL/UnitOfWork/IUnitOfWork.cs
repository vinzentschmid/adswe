using DAL.UnitOfWork.impl;

namespace DAL.UnitOfWork;

public interface IUnitOfWork
{
    DBContext Context { get; }
    
    IAquariumRepository AquariumRepository { get; }
}