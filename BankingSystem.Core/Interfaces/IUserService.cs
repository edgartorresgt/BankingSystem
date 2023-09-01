using BankingSystem.Models.Entities;

namespace BankingSystem.Core.Interfaces;

public interface IUserService
{
    Task<User> CreateUser(string name);
    Task<User?> GetUser(int userId);
    Task<List<User>?> GetAllUser();
    Task<bool> UpdateUser(int userId, string name);
    Task<bool> DeleteUser(int userId);
    Task<bool> Deposit(int accountId, decimal amount);
    Task<bool> Withdraw(int accountId, decimal amount);
}