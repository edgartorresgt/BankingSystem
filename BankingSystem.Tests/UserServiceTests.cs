using BankingSystem.Core.Interfaces;
using BankingSystem.Core.Services;
using BankingSystem.Models.Entities;
using BankingSystem.Repositories.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BankingSystem.Tests;

public class UserServiceTests
{
    [Fact]
    public async Task CreateUser_ShouldCreateUserAndAccount()
    {
        // Arrange
        var userRepositoryMock = new Mock<IGenericRepository<User>>();
        userRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<User>());

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.UserRepository).Returns(userRepositoryMock.Object);
        var accountServiceMock = new Mock<IAccountService>();

        var userService = new UserService(unitOfWorkMock.Object, accountServiceMock.Object);

        // Act
        var user = await userService.CreateUser("Edgar");

        // Assert
        Assert.NotNull(user);
        Assert.Equal("Edgar", user.Name);
        Assert.NotNull(user.Accounts);
    }

    [Fact]
    public async Task GetUser_ExistingUser_ShouldReturnUser()
    {
        // Arrange
        var userId = 1;
        var userRepositoryMock = new Mock<IGenericRepository<User>>();
        userRepositoryMock.Setup(repo => repo.GetById(userId)).Returns(new User { Id = userId, Name = "Dani" });

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.UserRepository).Returns(userRepositoryMock.Object);

        var userService = new UserService(unitOfWorkMock.Object, Mock.Of<IAccountService>());

        // Act
        var user = await userService.GetUser(userId);

        // Assert
        Assert.NotNull(user);
        Assert.Equal(userId, user.Id);
        Assert.Equal("Dani", user.Name);
    }

    [Fact]
    public async Task GetAllUser_NoUsers_ShouldReturnNull()
    {
        // Arrange
        var userRepositoryMock = new Mock<IGenericRepository<User>>();
        userRepositoryMock.Setup(repo => repo.GetAll()).Returns(Enumerable.Empty<User>());

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.UserRepository).Returns(userRepositoryMock.Object);

        var userService = new UserService(unitOfWorkMock.Object, Mock.Of<IAccountService>());

        // Act
        var users = await userService.GetAllUser();

        // Assert
        Assert.Null(users);
    }

    [Fact]
    public async Task DeleteUser_ExistingUser_ShouldDeleteUserAndAccounts()
    {
        // Arrange
        var userId = 1;
        var user = new User { Id = userId, Name = "Edgar", Accounts = new List<Account>() };
        var userRepositoryMock = new Mock<IGenericRepository<User>>();
        userRepositoryMock.Setup(repo => repo.GetById(userId)).Returns(user);

        var accountServiceMock = new Mock<IAccountService>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.UserRepository).Returns(userRepositoryMock.Object);

        var userService = new UserService(unitOfWorkMock.Object, accountServiceMock.Object);

        // Act
        var result = await userService.DeleteUser(userId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateUser_ExistingUser_ShouldUpdateUserName()
    {
        // Arrange
        var userId = 1;
        var userName = "UpdatedName";
        var user = new User { Id = userId, Name = "Edgar" };
        var userRepositoryMock = new Mock<IGenericRepository<User>>();
        userRepositoryMock.Setup(repo => repo.GetById(userId)).Returns(user);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.UserRepository).Returns(userRepositoryMock.Object);

        var userService = new UserService(unitOfWorkMock.Object, Mock.Of<IAccountService>());

        // Act
        var result = await userService.UpdateUser(userId, userName);

        // Assert
        Assert.True(result);
        Assert.Equal(userName, user.Name);
    }

    [Fact]
    public async Task UpdateUser_NonExistingUser_ShouldNotUpdate()
    {
        // Arrange
        var userId = 1;
        var userName = "UpdatedName";
        var userRepositoryMock = new Mock<IGenericRepository<User>>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.UserRepository).Returns(userRepositoryMock.Object);

        var userService = new UserService(unitOfWorkMock.Object, Mock.Of<IAccountService>());

        // Act
        var result = await userService.UpdateUser(userId, userName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Deposit_ValidAmount_ShouldDepositAndReturnTrue()
    {
        // Arrange
        var accountId = 1;
        var amount = 500;
        var account = new Account { Id = accountId, Balance = 1000 };
        var accountRepositoryMock = new Mock<IGenericRepository<Account>>();
        accountRepositoryMock.Setup(repo => repo.GetById(accountId)).Returns(account);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.AccountRepository).Returns(accountRepositoryMock.Object);

        var userService = new UserService(unitOfWorkMock.Object, Mock.Of<IAccountService>());

        // Act
        var result = await userService.Deposit(accountId, amount);

        // Assert
        Assert.True(result);
        Assert.Equal(1000 + amount, account.Balance);
    }

    [Fact]
    public async Task Deposit_InvalidAmount_ShouldNotDepositAndReturnFalse()
    {
        // Arrange
        var accountId = 1;
        var amount = -100;
        var accountRepositoryMock = new Mock<IGenericRepository<Account>>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.AccountRepository).Returns(accountRepositoryMock.Object);

        var userService = new UserService(unitOfWorkMock.Object, Mock.Of<IAccountService>());

        // Act
        var result = await userService.Deposit(accountId, amount);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Withdraw_ValidAmount_ShouldWithdrawAndReturnTrue()
    {
        // Arrange
        var accountId = 1;
        var amount = 300;
        var account = new Account { Id = accountId, Balance = 500 };
        var accountRepositoryMock = new Mock<IGenericRepository<Account>>();
        accountRepositoryMock.Setup(repo => repo.GetById(accountId)).Returns(account);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.AccountRepository).Returns(accountRepositoryMock.Object);

        var userService = new UserService(unitOfWorkMock.Object, Mock.Of<IAccountService>());

        // Act
        var result = await userService.Withdraw(accountId, amount);

        // Assert
        Assert.True(result);
        Assert.Equal(500 - amount, account.Balance);
    }

    [Fact]
    public async Task Withdraw_InvalidAmount_ShouldNotWithdrawAndReturnFalse()
    {
        // Arrange
        var accountId = 1;
        var amount = 10000;
        var accountRepositoryMock = new Mock<IGenericRepository<Account>>();
        accountRepositoryMock.Setup(repo => repo.GetById(accountId)).Returns(new Account { Id = accountId, Balance = 500 });

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.AccountRepository).Returns(accountRepositoryMock.Object);

        var userService = new UserService(unitOfWorkMock.Object, Mock.Of<IAccountService>());

        // Act
        var result = await userService.Withdraw(accountId, amount);

        // Assert
        Assert.False(result);
    }
}
