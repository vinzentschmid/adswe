using Serilog;
using Utils;

namespace UnitTests;

[TestFixture]
public class LoggerTests : BaseTest
{
    [Test]
    public void LoggerTest()
    {
        ILogger log = AquariumLogger.Logger;
        log.Fatal("Hilfe");
        Assert.That(true, Is.True);
    }
}