namespace DAL.UnitOfWork.impl;

public class AquariumRepository(DBContext context) : Repository<Aquarium>(context), IAquariumRepository
{
    public async Task<Aquarium> GetByName(string name)
    {
        return await FindOneAsync(x => x.Name.Equals(name));
    }
}