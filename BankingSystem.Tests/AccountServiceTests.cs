using System.Collections.Generic;
using BankingSystem.Core.Services;
using BankingSystem.Models.Entities;
using BankingSystem.Repositories.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BankingSystem.Tests;

public class AccountServiceTests
{
    [Fact]
    public async Task CreateAccount_ExistingUser_ShouldCreateAccountAndReturnIt()
    {
        // Arrange
        var userId = 1;
        var userRepositoryMock = new Mock<IGenericRepository<User>>();
        userRepositoryMock.Setup(repo => repo.GetById(userId)).Returns(new User { Id = userId });

        var accountRepositoryMock = new Mock<IGenericRepository<Account>>();
        accountRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Account>());

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.UserRepository).Returns(userRepositoryMock.Object);
        unitOfWorkMock.Setup(uow => uow.AccountRepository).Returns(accountRepositoryMock.Object);

        var accountService = new AccountService(unitOfWorkMock.Object);

        // Act
        var account = await accountService.CreateAccount(userId);

        // Assert
        Assert.NotNull(account);
        Assert.Equal(userId, account.UserId);
        Assert.Equal(100, account.Balance);
    }

    [Fact]
    public async Task CreateAccount_NonExistingUser_ShouldReturnNull()
    {
        // Arrange
        var userId = 1;
        var userRepositoryMock = new Mock<IGenericRepository<User>>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.UserRepository).Returns(userRepositoryMock.Object);

        var accountService = new AccountService(unitOfWorkMock.Object);

        // Act
        var account = await accountService.CreateAccount(userId);

        // Assert
        Assert.Null(account);
    }

    [Fact]
    public async Task DeleteAccount_ExistingAccountWithSufficientBalance_ShouldDeleteAccountAndReturnTrue()
    {
        // Arrange
        var accountId = 1;
        var account = new Account { Id = accountId, Balance = 150 };
        var accountRepositoryMock = new Mock<IGenericRepository<Account>>();
        accountRepositoryMock.Setup(repo => repo.GetById(accountId)).Returns(account);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.AccountRepository).Returns(accountRepositoryMock.Object);

        var accountService = new AccountService(unitOfWorkMock.Object);

        // Act
        var result = await accountService.DeleteAccount(accountId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAccount_ExistingAccountWithInsufficientBalance_ShouldNotDeleteAccountAndReturnFalse()
    {
        // Arrange
        var accountId = 1;
        var account = new Account { Id = accountId, Balance = 50 };
        var accountRepositoryMock = new Mock<IGenericRepository<Account>>();
        accountRepositoryMock.Setup(repo => repo.GetById(accountId)).Returns(account);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.AccountRepository).Returns(accountRepositoryMock.Object);

        var accountService = new AccountService(unitOfWorkMock.Object);

        // Act
        var result = await accountService.DeleteAccount(accountId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAccount_NonExistingAccount_ShouldReturnFalse()
    {
        // Arrange
        var accountId = 1;
        var accountRepositoryMock = new Mock<IGenericRepository<Account>>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.AccountRepository).Returns(accountRepositoryMock.Object);

        var accountService = new AccountService(unitOfWorkMock.Object);

        // Act
        var result = await accountService.DeleteAccount(accountId);

        // Assert
        Assert.False(result);
    }
}