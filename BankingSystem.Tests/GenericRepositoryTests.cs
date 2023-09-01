using BankingSystem.Repositories.Repositories;
using BankingSystem.Tests.TestModel;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BankingSystem.Tests;

public class GenericRepositoryTests
{
    [Fact]
    public void GetById_ExistingEntity_ShouldReturnEntity()
    {
        // Arrange
        var entities = new List<TestEntity>
        {
            new() {Id = 1},
            new() {Id = 2},
            new() {Id = 3}
        };

        var repository = new GenericRepository<TestEntity>();
        foreach (var testEntity in entities) repository.Add(testEntity);

        // Act
        var result = repository.GetById(2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Id);
    }

    [Fact]
    public void GetById_NonExistingEntity_ShouldReturnNull()
    {
        // Arrange
        var repository = new GenericRepository<TestEntity>();

        // Act
        var result = repository.GetById(2);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetAll_ShouldReturnAllEntities()
    {
        // Arrange
        var entities = new List<TestEntity>
        {
            new() {Id = 1},
            new() {Id = 2},
            new() {Id = 3}
        };

        var repository = new GenericRepository<TestEntity>();
        foreach (var testEntity in entities) repository.Add(testEntity);

        // Act
        var result = repository.GetAll();

        // Assert
        Assert.Equal(entities.Count, result.Count());
    }

    [Fact]
    public void Add_ShouldAddEntity()
    {
        // Arrange
        var repository = new GenericRepository<TestEntity>();
        var entity = new TestEntity { Id = 1 };

        // Act
        repository.Add(entity);

        // Assert
        var result = repository.GetById(1);
        Assert.NotNull(result);
        Assert.Equal(entity.Id, result.Id);
    }

    [Fact]
    public void Update_ExistingEntity_ShouldUpdateEntity()
    {
        // Arrange
        var entities = new List<TestEntity>
            {
                new() { Id = 1 },
                new() { Id = 2 }
            };

        var repository = new GenericRepository<TestEntity>();
        foreach (var testEntity in entities) repository.Add(testEntity);

        var updatedEntity = new TestEntity { Id = 2 };

        // Act
        repository.Update(updatedEntity);

        // Assert
        var result = repository.GetById(2);
        Assert.NotNull(result);
        Assert.Equal(updatedEntity.Id, result.Id);
    }

    [Fact]
    public void Update_NonExistingEntity_ShouldNotUpdate()
    {
        // Arrange
        var repository = new GenericRepository<TestEntity>();
        var entity = new TestEntity { Id = 1 };

        // Act
        repository.Update(entity);

        // Assert
        var result = repository.GetById(1);
        Assert.Null(result);
    }

    [Fact]
    public void Delete_ExistingEntity_ShouldDeleteEntity()
    {
        // Arrange
        var entities = new List<TestEntity>
            {
                new() { Id = 1 },
                new() { Id = 2 }
            };

        var repository = new GenericRepository<TestEntity>();
        foreach (var testEntity in entities) repository.Add(testEntity);

        var entityToDelete = repository.GetById(2);

        // Act
        repository.Delete(entityToDelete!);

        // Assert
        var result = repository.GetById(2);
        Assert.Null(result);
    }

 
}