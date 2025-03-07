using System.Linq.Expressions;
using DAL;
using DAL.UnitOfWork;
using DAL.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Serilog;
using Services;
using Services.Authentication;
using Services.Models;
using Utils;

namespace UnitTests;

public class Tests
{
    private ISettings Settings;
    private ISettingsHandler SettingsHandler;
    private AquariumLogger AquariumLogger;
    private IPasswordHasher PasswordHasher;
    private IUnitOfWork UnitOfWork;
    private IAuthentication Authentication;
    
    [SetUp]
    public async Task Setup()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<ISettings, AquariumSettings>();
        //serviceCollection.AddSingleton<ISettingsHandler, TestSettingsHandler>();
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
        //AquariumLogger = serviceProvider.GetRequiredService<IAquariumLogger>();
        //await AquariumLogger.Init();
        PasswordHasher = serviceProvider.GetRequiredService<IPasswordHasher>();
        UnitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        Authentication = serviceProvider.GetRequiredService<IAuthentication>();


    }


    
    
 
}