using BankingSystem.Repositories.Interfaces;

namespace BankingSystem.Repositories.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private List<TEntity> _data = new();

    public TEntity? GetById(int id)
    {
        return _data.FirstOrDefault(e => (e as dynamic).Id == id);
    }

    public IEnumerable<TEntity> GetAll()
    {
        if (!_data.Any())
        {
            _data = new List<TEntity>();
        }
        return _data;
    }

    public void Add(TEntity entity)
    {
        _data.Add(entity);
    }

    public void Update(TEntity entity)
    {
        var existingEntity = GetById((entity as dynamic).Id);
        if (existingEntity == null) return;
        _data.Remove(existingEntity);
        _data.Add(entity);
    }

    public void Delete(TEntity entity)
    {
        _data.Remove(entity);
    }
}
