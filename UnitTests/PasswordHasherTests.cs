using DAL;
using DAL.Utils;

namespace UnitTests;

public class PasswordHasherTests
{
    [Test]
    public void Hash_ShouldReturnHashedPassword()
    {
        var password = "TestPassword";
        
        PasswordHasher hasher = new PasswordHasher();
        
        var hash = hasher.Hash(password);

        Boolean result = hasher.Check(hash, password);
        
        Assert.That(result, Is.True);
    }
}