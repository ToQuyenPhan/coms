using System.Linq.Expressions;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IGenericRepository<T> where T : class
    {
        IList<T> GetAll();
        Task CreateAsync(T entity);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes);
        Task<IList<T>> WhereAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes);
        Task<IList<T>> WhereAsyncWithParam(Expression<Func<T, bool>> predicate, params string[] navigationProperties);
        Task CreateRangeAsync(List<T> entities);
        Task UpdateAsync(T updated);
        Task UpdateRangeAsync(IList<T> entities);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IList<T> entities);
        Task<IList<T>> WhereAsyncWithFilter(Expression<Func<T, bool>> predicate,
                Expression<Func<T, object>>[]? includes, int currentPage, int pageSize);
    }
}
