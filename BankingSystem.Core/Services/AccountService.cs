using BankingSystem.Core.Interfaces;
using BankingSystem.Models.Entities;
using BankingSystem.Repositories.Interfaces;

namespace BankingSystem.Core.Services;

public class AccountService : IAccountService
{
    private readonly IUnitOfWork _unitOfWork;

    public AccountService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Account> CreateAccount(int userId)
    {
        var user = _unitOfWork.UserRepository.GetById(userId);
        if (user == null)
            return null!;

        var accountId = 0;
        var accounts = _unitOfWork.AccountRepository.GetAll().Where(x => x.UserId == userId);

        var accountsPerUserId = accounts.ToList();
        accountId = accountsPerUserId.Any() ? accountsPerUserId.Max(x => x.Id) + 1 : 1;

        var newAccount = new Account { Id = accountId, UserId = userId, Balance = 100 };
        user.Accounts.Add(newAccount);
        await  _unitOfWork.SaveChangesAsync();
        return newAccount;
    }

    public async Task<bool>  DeleteAccount(int accountId)
    {
        var account = _unitOfWork.AccountRepository.GetById(accountId);
        if (account == null)
            return false;

        if (account.Balance < 100) return false;

        _unitOfWork.AccountRepository.Delete(account);
        await _unitOfWork.SaveChangesAsync();
        return true;

    }
}