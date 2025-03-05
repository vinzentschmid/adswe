namespace DAL.UnitOfWork.impl;

public interface IUserRepository : IRepository<User>
{
    Task<User> Login(String username, String password);
}