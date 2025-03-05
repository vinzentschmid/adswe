using DAL.Utils;

namespace DAL.UnitOfWork.impl;

public class UserRepository(DBContext context, IPasswordHasher passwordHasher): Repository<User>(context), IUserRepository
{
    public async Task<User> Login(string username, string password)
    {
        var user = await FindOneAsync(u => u.Email == username);
        if (!passwordHasher.Check(user.HashedPassword, password))
        {
            throw new UnauthorizedAccessException("Invalid username or password.");
        }
        return user;
    }
}