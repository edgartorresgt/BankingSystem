using BankingSystem.Models.Entities;

namespace BankingSystem.Core.Interfaces;

public interface IAccountService
{
    Task<Account> CreateAccount(int userId);
    Task<bool> DeleteAccount(int accountId);
}