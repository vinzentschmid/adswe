using System.Linq.Expressions;
using DAL;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Services;
using Services.Models;

namespace UnitTests;

[TestFixture]
public class AquariumTests : BaseTest
{
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
        Assert.That(result, Is.Null);
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
        Assert.Multiple(() =>
        {
            Assert.That(retrievedDocument.ID, Is.EqualTo(insertedDocument.ID));
            Assert.That(retrievedDocument.Name, Is.EqualTo(insertedDocument.Name));
        });
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
        Assert.Multiple(() =>
        {
            Assert.That(retrievedDocument.ID, Is.EqualTo(document.ID));
            Assert.That(retrievedDocument.Name, Is.EqualTo(document.Name));
        });
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
        var enumerable = projectedDocuments as dynamic[] ?? projectedDocuments.ToArray();
        Assert.That(enumerable, Is.Not.Null);
        Assert.That(enumerable.Any(d => d.Name == "TestAquarium1" && d.Liters == 572), Is.True);
        Assert.That(enumerable.Any(d => d.Name == "TestAquarium2" && d.Liters == 720), Is.True);
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
        var enumerable = filteredDocuments as Aquarium[] ?? filteredDocuments.ToArray();
        Assert.That(enumerable, Is.Not.Null);
        //Assert.That(filteredDocuments.Count(), Is.EqualTo(1));
        Assert.That(enumerable.Any(d => d is { Name: "TestAquarium2", Liters: 720 }), Is.True);
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

        var service = new AquariumService(UnitOfWork, UnitOfWork.AquariumRepository);
        await service.Load("test@example.com");
        
        var modelState = new Mock<ModelStateDictionary>();
        await service.SetModelState(modelState.Object);
        
        var model = await service.Create(document);
        
        Assert.That(model.HasError, Is.False);
    }
    
    [Test]
    public async Task GetByName_ValidName_ReturnsAquarium()
    {
        // Arrange
        const string name = "Ocean World";
        var aquarium = new Aquarium { Name = name };

        // Insert the aquarium into the database
        await UnitOfWork.AquariumRepository.InsertOneAsync(aquarium);

        // Act
        var result = await UnitOfWork.AquariumRepository.GetByName(name);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Name, Is.EqualTo(name));
    }
}
