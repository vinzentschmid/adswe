namespace DAL.UnitOfWork.impl;

public interface IAquariumRepository : IRepository<Aquarium>
{
    Task<Aquarium> GetByName(String name);
}