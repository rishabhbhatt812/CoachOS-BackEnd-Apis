using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CoachOS.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id)
        => await _dbSet.FindAsync(id);

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool ignoreQueryFilters = false)
    {
        IQueryable<T> query = _dbSet;
        if (ignoreQueryFilters) query = query.IgnoreQueryFilters();
        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<List<T>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public void Update(T entity)
        => _dbSet.Update(entity);

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, bool ignoreQueryFilters = false)
    {
        IQueryable<T> query = _dbSet;
        if (ignoreQueryFilters) query = query.IgnoreQueryFilters();
        return await query.AnyAsync(predicate);
    }

    public void Remove(T entity)
        => _dbSet.Remove(entity);

    public async Task<CoachOS.Shared.Responses.PagedResult<T>> GetPagedAsync(CoachOS.Shared.Requests.PaginationParams pagination, Expression<Func<T, bool>>? predicate = null)
    {
        IQueryable<T> query = _dbSet;
        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        var totalCount = await query.CountAsync();
        var items = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize)
                               .Take(pagination.PageSize)
                               .ToListAsync();

        return new CoachOS.Shared.Responses.PagedResult<T>(items, totalCount, pagination.PageNumber, pagination.PageSize);
    }
}