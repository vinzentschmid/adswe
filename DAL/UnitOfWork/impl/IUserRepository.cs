namespace DAL.UnitOfWork.impl;

public interface IUserRepository : IRepository<User>
{
    Task<User> Login(string username, string password);
    
    Task<User> FindByUsernameAsync(string username);
}