using BankingSystem.Models.Entities;
using BankingSystem.Repositories.Interfaces;

namespace BankingSystem.Repositories.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(IGenericRepository<Account> accountGenericRepository,
        IGenericRepository<User> userGenericRepository)
    { 
        AccountRepository = accountGenericRepository;
        UserRepository = userGenericRepository;
    }

    public IGenericRepository<Account> AccountRepository { get; }
    public IGenericRepository<User> UserRepository { get; }

    public Task SaveChangesAsync()
    {
        Console.WriteLine("Since I'm not dealing with persistence in a database or any other, this approach is kind of different because everything is in the scope of in-memory lists.");
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

}