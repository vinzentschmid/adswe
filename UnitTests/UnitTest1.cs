using DAL;
using DAL.UnitOfWork;
using DAL.Utils;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Utils;

namespace UnitTests;

public class Tests
{
    protected ISettings Settings;
    protected ISettingsHandler SettingsHandler;
    protected AquariumLogger AquariumLogger;
    protected IPasswordHasher PasswordHasher;
    protected IUnitOfWork UnitOfWork;
    
    [SetUp]
    public async Task Setup()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<ISettings, AquariumSettings>();
        //serviceCollection.AddSingleton<ISettingsHandler, TestSettingsHandler>();
        serviceCollection.AddSingleton<IPasswordHasher, PasswordHasher>();
        serviceCollection.AddSingleton<ISettingsHandler, ConsulSettingsHandler>();
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();


        var serviceProvider = serviceCollection.BuildServiceProvider();


        Settings = serviceProvider.GetRequiredService<ISettings>();
        SettingsHandler = serviceProvider.GetRequiredService<ISettingsHandler>();
        
        await SettingsHandler.Load();
      
  

        AquariumLogger = new AquariumLogger(Settings);
        await AquariumLogger.Initialize();
        //AquariumLogger = serviceProvider.GetRequiredService<IAquariumLogger>();
        //await AquariumLogger.Init();
        PasswordHasher = serviceProvider.GetRequiredService<IPasswordHasher>();
        UnitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

    }

    [Test]
    public void Test1()
    {
        ILogger log = AquariumLogger.Logger;
        log.Fatal("Hilfe");
        Assert.That(true, Is.True);
    }
    
    [Test]
    public void Hash_ShouldReturnHashedPassword()
    {
        var password = "TestPassword";
        
        var hash = PasswordHasher.Hash(password);

        Boolean result = PasswordHasher.Check(hash, password);
        
        Assert.That(result, Is.True);
    }

    [Test]
    public void TestDBConnection()
    {
        DBContext context = new DBContext(Settings);
        Assert.That(context.IsConnected, Is.True);
    }

    [Test]
    public async Task CreateAndReadAquarium()
    {
        Aquarium aquarium = new Aquarium();
        aquarium.Name = "TestAquarium";
        aquarium.Depth = 55;
        aquarium.Height = 55;
        aquarium.Length = 55;
        aquarium.Liters = 572;
        aquarium.WaterType = WaterType.Freshwater;
        
        Aquarium createdAquarium = await UnitOfWork.AquariumRepository.InsertOneAsync(aquarium);
        
        Assert.That(createdAquarium, Is.Not.Null);
    }
}