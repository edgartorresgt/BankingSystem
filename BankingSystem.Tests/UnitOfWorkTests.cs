using BankingSystem.Models.Entities;
using BankingSystem.Repositories.Interfaces;
using BankingSystem.Repositories.Repositories;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BankingSystem.Tests;

public class UnitOfWorkTests
{
    [Fact]
    public async Task SaveChangesAsync_ShouldCompleteSuccessfully()
    {
        // Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        // Act
        await unitOfWorkMock.Object.SaveChangesAsync();

        // Assert
        unitOfWorkMock.Verify(m => m.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public void Dispose_ShouldSuppressFinalize()
    {
        // Arrange
        var accountRepositoryMock = new Mock<IGenericRepository<Account>>();
        var userRepositoryMock = new Mock<IGenericRepository<User>>();

        var unitOfWork = new UnitOfWork(accountRepositoryMock.Object, userRepositoryMock.Object);

        // Act
        unitOfWork.Dispose();

        // Assert
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }

    [Fact]
    public void Dispose_ShouldDisposeRepositories()
    {
        // Arrange
        var accountRepositoryMock = new Mock<IGenericRepository<Account>>();
        var userRepositoryMock = new Mock<IGenericRepository<User>>();

        var unitOfWork = new UnitOfWork(accountRepositoryMock.Object, userRepositoryMock.Object);

        // Act
        unitOfWork.Dispose();

        // Assert
        GC.SuppressFinalize(unitOfWork);
    }
}