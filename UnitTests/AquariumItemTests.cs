using DAL;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Services;

namespace UnitTests;

[TestFixture]
public class AquariumItemTests : BaseTest
{
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
        var getAnimal = await service.GetAnimal(document.ID);
        Assert.That(getAnimal, Is.Not.Null);
        Assert.That(result.HasError, Is.False);
    }
}