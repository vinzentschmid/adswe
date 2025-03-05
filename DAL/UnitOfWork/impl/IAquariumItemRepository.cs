namespace DAL.UnitOfWork.impl;

public interface IAquariumItemRepository : IRepository<AquariumItem>
{
    List<Coral> GetCorals();
    List<Animal> GetAnimals();
}