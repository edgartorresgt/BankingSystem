using BankingSystem.Core.Interfaces;
using BankingSystem.Models.Entities;
using BankingSystem.Repositories.Interfaces;

namespace BankingSystem.Core.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccountService _accountService;

    public UserService(IUnitOfWork unitOfWork, IAccountService accountService)
    {
        _unitOfWork = unitOfWork;
        _accountService = accountService;
    }

    public async Task<User> CreateUser(string name)
    {
        var allUsers = _unitOfWork.UserRepository.GetAll();
        var userId = allUsers.Any() ? allUsers.ToList().Max(user => user.Id) + 1 : 1;

        var newUser = new User {Id = userId, Name = name, Accounts = new List<Account>()};
        _unitOfWork.UserRepository.Add(newUser);

         await _accountService.CreateAccount(userId);
         await _unitOfWork.SaveChangesAsync();
        return newUser;
    }

    public async Task<User?> GetUser(int userId)
    {
        var user = _unitOfWork.UserRepository.GetById(userId);
        if (user == null)
            return null;

        return await Task.FromResult(user) ;
    }

    public async Task<List<User>?> GetAllUser()
    {
        var allUsers = _unitOfWork.UserRepository.GetAll();
        var result = allUsers.ToList();
        if (!result.Any())
            return null;

        return await Task.FromResult(result);
    }

    public async Task<bool> DeleteUser(int userId)
    {
        var user = _unitOfWork.UserRepository.GetById(userId);
        if (user == null)
            return false;

        // Delete user's accounts first
        foreach (var account in user.Accounts.ToList())
        {
           await _accountService.DeleteAccount(account.Id);
        }

        _unitOfWork.UserRepository.Delete(user);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateUser(int userId, string name)
    {
        var account = _unitOfWork.UserRepository.GetById(userId);
        if (account == null)
            return false;

        account.Name = name;
        _unitOfWork.UserRepository.Update(account);

        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Deposit(int accountId, decimal amount)
    {
        if (amount is <= 0 or > 10000)
            return false;

        var account = _unitOfWork.AccountRepository.GetById(accountId);
        if (account == null)
            return false;

        account.Balance += amount;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Withdraw(int accountId, decimal amount)
    {
        var account = _unitOfWork.AccountRepository.GetById(accountId);
        if (account == null || amount <= 0 || amount > account.Balance * 0.9m || account.Balance - amount < 100)
            return false;

        account.Balance -= amount;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

}