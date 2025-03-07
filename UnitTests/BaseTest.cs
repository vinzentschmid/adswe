using DAL.UnitOfWork;
using DAL.Utils;
using Microsoft.Extensions.DependencyInjection;
using Services.Authentication;
using Utils;

namespace UnitTests;

public class BaseTest
{
    protected ISettings Settings;
    protected ISettingsHandler SettingsHandler;
    protected AquariumLogger AquariumLogger;
    protected IPasswordHasher PasswordHasher;
    protected IUnitOfWork UnitOfWork;
    protected IAuthentication Authentication;
    
    [SetUp]
    public async Task Setup()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<ISettings, AquariumSettings>();
        serviceCollection.AddSingleton<IPasswordHasher, PasswordHasher>();
        serviceCollection.AddSingleton<ISettingsHandler, ConsulSettingsHandler>();
        serviceCollection.AddSingleton<IAuthentication, Authentication>();
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        Settings = serviceProvider.GetRequiredService<ISettings>();
        SettingsHandler = serviceProvider.GetRequiredService<ISettingsHandler>();
        await SettingsHandler.Load();

        AquariumLogger = new AquariumLogger(Settings);
        await AquariumLogger.Initialize();
        PasswordHasher = serviceProvider.GetRequiredService<IPasswordHasher>();
        UnitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        Authentication = serviceProvider.GetRequiredService<IAuthentication>();
    }
    
    [TearDown]
    public async Task Teardown()
    {
        //Expression<Func<Aquarium, bool>> filterExpressionAquarium = x => true;
        //await UnitOfWork.AquariumRepository.DeleteOneAsync(filterExpressionAquarium);
        //Expression<Func<User, bool>> filterExpressionUser = x => true;
        //await UnitOfWork.UserRepository.DeleteOneAsync(filterExpressionUser);
        //Expression<Func<AquariumItem, bool>> filterExpressionAquariumItem = x => true;
        //await UnitOfWork.AquariumItemRepository.DeleteOneAsync(filterExpressionAquariumItem);
    }
}