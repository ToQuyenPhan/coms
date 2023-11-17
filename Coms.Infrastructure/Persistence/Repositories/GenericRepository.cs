using Coms.Application.Common.Intefaces.Persistence;
using Coms.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ComsDBContext _dbContext;
        private DbSet<T> _entities;

        public GenericRepository(ComsDBContext context)
        {
            this._dbContext = context;
            _entities = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _entities.ToList();
        }

        public virtual async Task CreateAsync(T entity)
        {
            await _entities.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateRangeAsync(List<T> entities)
        {
            await _dbContext.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T updated)
        {
            _dbContext.Attach(updated).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IList<T> entities)
        {
            _dbContext.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _entities.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteRangeAsync(IList<T> entities)
        {
            _entities.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.AsQueryable().AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes)
        {
            return await AsQueryableWithIncludes(includes).AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<IList<T>> WhereAsyncWithParam(Expression<Func<T, bool>> predicate, params string[] navigationProperties)
        {
            List<T> list;
            var query = _entities.AsQueryable();
            foreach (string navigationProperty in navigationProperties)
                query = query.Include(navigationProperty);
            list = await query.Where(predicate).AsNoTracking().ToListAsync<T>();
            return list;
        }

        public async Task<IList<T>> WhereAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes)
        {
            return await AsQueryableWithIncludes(includes).Where(predicate).AsNoTracking().ToListAsync();
        }

        private IQueryable<T> AsQueryableWithIncludes(Expression<Func<T, object>>[]? includes)
        {
            var query = _entities.AsQueryable();
            if (includes == null) return query;

            foreach (var item in includes)
            {
                query = query.Include(item);
            }

            return query;
        }
    }
}
