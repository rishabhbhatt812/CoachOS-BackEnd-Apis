using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CoachOS.Application.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool ignoreQueryFilters = false);
        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, bool ignoreQueryFilters = false);
        Task<CoachOS.Shared.Responses.PagedResult<T>> GetPagedAsync(CoachOS.Shared.Requests.PaginationParams pagination, Expression<Func<T, bool>>? predicate = null);
    }
}
