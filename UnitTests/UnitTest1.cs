using System.Linq.Expressions;
using DAL;
using DAL.UnitOfWork;
using DAL.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Serilog;
using Services;
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
    public void LoggerTest()
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
        var aquarium = new Aquarium
        {
            Name = "TestAquarium",
            Depth = 55,
            Height = 55,
            Length = 55,
            Liters = 572,
            WaterType = WaterType.Freshwater
        };

        var createdAquarium = await UnitOfWork.AquariumRepository.InsertOneAsync(aquarium);
        
        Assert.That(createdAquarium, Is.Not.Null);
    }
    [Test]
    public async Task DeleteOneAsync_ShouldDeleteDocument()
    {
        // Arrange
        var document = new Aquarium
        {
            Name = "TestAquarium",
            Depth = 55,
            Height = 55,
            Length = 55,
            Liters = 572,
            WaterType = WaterType.Freshwater
        };
        await UnitOfWork.AquariumRepository.InsertOneAsync(document);

        // Act
        Expression<Func<Aquarium, bool>> filterExpression = x => x.ID == document.ID;
        await UnitOfWork.AquariumRepository.DeleteOneAsync(filterExpression);

        // Assert
        var result = await UnitOfWork.AquariumRepository.FindOneAsync(filterExpression);
        Assert.IsNull(result);
    }
    [Test]
    public async Task InsertOrUpdateOneAsync_ShouldInsertOrUpdateDocument()
    {
        // Arrange
        var document = new Aquarium
        {
            Name = "TestAquarium",
            Depth = 55,
            Height = 55,
            Length = 55,
            Liters = 572,
            WaterType = WaterType.Freshwater
        };

        // Act - Insert
        var insertedDocument = await UnitOfWork.AquariumRepository.InsertOrUpdateOneAsync(document);

        // Assert - Insert
        Assert.That(insertedDocument, Is.Not.Null);

        // Modify the document
        insertedDocument.Name = "UpdatedAquarium";

        // Act - Update
        var updatedDocument = await UnitOfWork.AquariumRepository.InsertOrUpdateOneAsync(insertedDocument);

        // Assert - Update
        Assert.That(updatedDocument, Is.Not.Null);
        Assert.That(updatedDocument.Name, Is.EqualTo("UpdatedAquarium"));
    }
    [Test]
    public async Task UpdateOneAsync_ShouldUpdateDocument()
    {
        // Arrange
        var document = new Aquarium
        {
            Name = "TestAquarium",
            Depth = 55,
            Height = 55,
            Length = 55,
            Liters = 572,
            WaterType = WaterType.Freshwater
        };
        await UnitOfWork.AquariumRepository.InsertOneAsync(document);

        // Modify the document
        document.Name = "UpdatedAquarium";

        // Act
        var updatedDocument = await UnitOfWork.AquariumRepository.UpdateOneAsync(document);

        // Assert
        Assert.That(updatedDocument, Is.Not.Null);
        Assert.That(updatedDocument.Name, Is.EqualTo("UpdatedAquarium"));

        // Verify the update in the database
        var retrievedDocument = await UnitOfWork.AquariumRepository.FindByIdAsync(updatedDocument.ID.ToString());
        Assert.That(retrievedDocument, Is.Not.Null);
        Assert.That(retrievedDocument.Name, Is.EqualTo("UpdatedAquarium"));
    }
    [Test]
    public async Task FindByIdAsync_ShouldReturnDocument()
    {
        // Arrange
        var document = new Aquarium
        {
            Name = "TestAquarium",
            Depth = 55,
            Height = 55,
            Length = 55,
            Liters = 572,
            WaterType = WaterType.Freshwater
        };
        var insertedDocument = await UnitOfWork.AquariumRepository.InsertOneAsync(document);

        // Act
        var retrievedDocument = await UnitOfWork.AquariumRepository.FindByIdAsync(insertedDocument.ID.ToString());

        // Assert
        Assert.That(retrievedDocument, Is.Not.Null);
        Assert.That(retrievedDocument.ID, Is.EqualTo(insertedDocument.ID));
        Assert.That(retrievedDocument.Name, Is.EqualTo(insertedDocument.Name));
    }
    [Test]
    public async Task FindOneAsync_ShouldReturnDocument()
    {
        // Arrange
        var document = new Aquarium
        {
            Name = "TestAquarium",
            Depth = 55,
            Height = 55,
            Length = 55,
            Liters = 572,
            WaterType = WaterType.Freshwater
        };
        await UnitOfWork.AquariumRepository.InsertOneAsync(document);

        // Act
        Expression<Func<Aquarium, bool>> filterExpression = x => x.ID == document.ID;
        var retrievedDocument = await UnitOfWork.AquariumRepository.FindOneAsync(filterExpression);

        // Assert
        Assert.That(retrievedDocument, Is.Not.Null);
        Assert.That(retrievedDocument.ID, Is.EqualTo(document.ID));
        Assert.That(retrievedDocument.Name, Is.EqualTo(document.Name));
    }
    
    [Test]
    public async Task FilterBy_ShouldReturnProjectedDocuments()
    {
        // Arrange
        var document1 = new Aquarium
        {
            Name = "TestAquarium1",
            Depth = 55,
            Height = 55,
            Length = 55,
            Liters = 572,
            WaterType = WaterType.Freshwater
        };
        var document2 = new Aquarium
        {
            Name = "TestAquarium2",
            Depth = 60,
            Height = 60,
            Length = 60,
            Liters = 720,
            WaterType = WaterType.Saltwater
        };
        await UnitOfWork.AquariumRepository.InsertOneAsync(document1);
        await UnitOfWork.AquariumRepository.InsertOneAsync(document2);

        // Act
        Expression<Func<Aquarium, bool>> filterExpression = x => x.Liters > 500;
        Expression<Func<Aquarium, dynamic>> projectionExpression = x => new { x.Name, x.Liters };
        var projectedDocuments = UnitOfWork.AquariumRepository.FilterBy(filterExpression, projectionExpression);

        // Assert
        Assert.That(projectedDocuments, Is.Not.Null);
        Assert.That(projectedDocuments.Any(d => d.Name == "TestAquarium1" && d.Liters == 572), Is.True);
        Assert.That(projectedDocuments.Any(d => d.Name == "TestAquarium2" && d.Liters == 720), Is.True);
    }
    
    [Test]
    public async Task FilterBy_ShouldReturnFilteredDocuments()
    {
        // Arrange
        var document1 = new Aquarium
        {
            Name = "TestAquarium1",
            Depth = 55,
            Height = 55,
            Length = 55,
            Liters = 572,
            WaterType = WaterType.Freshwater
        };
        var document2 = new Aquarium
        {
            Name = "TestAquarium2",
            Depth = 60,
            Height = 60,
            Length = 60,
            Liters = 720,
            WaterType = WaterType.Saltwater
        };
        await UnitOfWork.AquariumRepository.InsertOneAsync(document1);
        await UnitOfWork.AquariumRepository.InsertOneAsync(document2);

        // Act
        Expression<Func<Aquarium, bool>> filterExpression = x => x.Liters > 600;
        var filteredDocuments = UnitOfWork.AquariumRepository.FilterBy(filterExpression);

        // Assert
        Assert.That(filteredDocuments, Is.Not.Null);
        //Assert.That(filteredDocuments.Count(), Is.EqualTo(1));
        Assert.That(filteredDocuments.Any(d => d.Name == "TestAquarium2" && d.Liters == 720), Is.True);
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
        Assert.AreEqual(username, result.Email);
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
    [Test]
    public async Task GetByName_ValidName_ReturnsAquarium()
    {
        // Arrange
        var name = "Ocean World";
        var aquarium = new Aquarium { Name = name };

        // Insert the aquarium into the database
        await UnitOfWork.AquariumRepository.InsertOneAsync(aquarium);

        // Act
        var result = await UnitOfWork.AquariumRepository.GetByName(name);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Name, Is.EqualTo(name));
    }
    
    [Test]
    public async Task GetCorals_ReturnsCorals()
    {
        // Arrange
        var coral = new Coral { Name = "Coral1", CoralType = CoralType.SoftCoral };
        await UnitOfWork.AquariumItemRepository.InsertOneAsync(coral);

        // Act
        var result = UnitOfWork.AquariumItemRepository.GetCorals();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotEmpty(result);
        Assert.IsInstanceOf<List<Coral>>(result);
        //Assert.That(result[0].Name, Is.EqualTo(coral.Name));
    }
    [Test]
    public async Task GetAnimals_ReturnsAnimals()
    {
        // Arrange
        var animal = new Animal { Name = "Animal1", IsAlive = true };
        await UnitOfWork.AquariumItemRepository.InsertOneAsync(animal);

        // Act
        var result = UnitOfWork.AquariumItemRepository.GetAnimals();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotEmpty(result);
        Assert.IsInstanceOf<List<Animal>>(result);
        Assert.That(result[0].Name, Is.EqualTo(animal.Name));
    }

    [Test]
    public async Task CreateOverService()
    {
        var document = new Aquarium
        {
            Name = "TestAquarium",
            Depth = 55,
            Height = 55,
            Length = 55,
            Liters = 572,
            WaterType = WaterType.Freshwater
        };

        AquariumService service = new AquariumService(UnitOfWork, UnitOfWork.AquariumRepository);
        await service.Load("test@example.com");
        
        var modelState = new Mock<ModelStateDictionary>();
        await service.SetModelState(modelState.Object);
        
        ItemResponseModel<Aquarium> model = await service.Create(document);
        
        Assert.That(model.HasError, Is.False);
    }
    [Test]
    public async Task CreateCoralOverService()
    {
        var document = new Coral
        {
            Name = "TestCoral",
            Species = "TestSpecies",
            
        };
        CoralService service = new CoralService(UnitOfWork, UnitOfWork.AquariumItemRepository);
        
        var modelState = new Mock<ModelStateDictionary>();
        await service.SetModelState(modelState.Object);
        
        var result = await service.AddCoral(document);
        var getCoral = await service.GetCoral(document.ID);
        Assert.That(getCoral, Is.Not.Null);
        Assert.That(result.HasError, Is.False);
    }
    [Test]
    public async Task CreateAnimalOverService()
    {
        var document = new Animal
        {
            Name = "TestAnimal",
            Species = "TestSpecies",
            
        };
        AnimalService service = new AnimalService(UnitOfWork, UnitOfWork.AquariumItemRepository);
        
        var modelState = new Mock<ModelStateDictionary>();
        await service.SetModelState(modelState.Object);
        
        var result = await service.AddAnimal(document);
        
        Assert.That(result.HasError, Is.False);
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