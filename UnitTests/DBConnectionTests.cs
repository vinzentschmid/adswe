using DAL;

namespace UnitTests;

[TestFixture]
public class DBConnectionTests : BaseTest
{
    [Test]
    public void TestDBConnection()
    {
        var context = new DBContext(Settings);
        Assert.That(context.IsConnected, Is.True);
    }
}