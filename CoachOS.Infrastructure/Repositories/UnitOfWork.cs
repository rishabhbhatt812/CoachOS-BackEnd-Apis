using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Infrastructure.Data;
using System.Collections;

namespace CoachOS.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly Hashtable _repositories = new();

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repository = new Repository<T>(_context);
            _repositories.Add(type, repository);
        }

        return (IRepository<T>)_repositories[type]!;
    }

    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();
}