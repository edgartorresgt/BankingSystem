using BankingSystem.Models.Entities;

namespace BankingSystem.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Account> AccountRepository { get; }
    IGenericRepository<User> UserRepository { get; }
    Task SaveChangesAsync();
}