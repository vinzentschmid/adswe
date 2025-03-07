using DAL;
using Services;

namespace UnitTests;

[TestFixture]
public class AuthenticationTests : BaseTest
{
    [Test]
    public async Task TestLogin()
    {
        UserService service = new UserService(UnitOfWork, UnitOfWork.UserRepository, Authentication);
        LoginRequest request = new LoginRequest { Password = "password", Username = "test@example.com" };
        var response = await service.Login(request);
        Assert.That(response.HasError, Is.False);
    }
    
    [Test]
    public async Task Login_ValidCredentials_ReturnsUser()
    {
        // Arrange
        var username = "test@example.com";
        var password = "password";
        var hashedPassword = PasswordHasher.Hash(password);
        var user = new User { Email = username, HashedPassword = hashedPassword };

        // Insert the user into the database
        await UnitOfWork.UserRepository.InsertOneAsync(user);

        // Act
        var result = await UnitOfWork.UserRepository.Login(username, password);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Email, Is.EqualTo(username));
    }
    
    [Test]
    public async Task Login_InvalidCredentials_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var username = "test@example.com";
        var password = "password";
        var hashedPassword = PasswordHasher.Hash(password);
        var user = new User { Email = username, HashedPassword = hashedPassword };

        // Insert the user into the database
        await UnitOfWork.UserRepository.InsertOneAsync(user);

        // Act & Assert
        Assert.ThrowsAsync<UnauthorizedAccessException>(() => UnitOfWork.UserRepository.Login(username, "wrongpassword"));
    }
}