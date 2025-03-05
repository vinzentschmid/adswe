namespace DAL.UnitOfWork.impl;

public class AquariumItemRepository(DBContext context) : Repository<AquariumItem>(context), IAquariumItemRepository
{
    public List<Coral> GetCorals()
    {
        return FilterBy(item => item is Coral)
            .Select(item => (Coral)item)
            .ToList();
    }

    public List<Animal> GetAnimals()
    {
        return FilterBy(item => item is Animal)
            .Select(item => (Animal)item)
            .ToList();
    }
}