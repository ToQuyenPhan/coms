using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using Coms.Domain.Enum;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IGenericRepository<User> _genericRepository;

        public UserRepository(IGenericRepository<User> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _genericRepository.FirstOrDefaultAsync(u => u.Username == username, 
                    new System.Linq.Expressions.Expression<Func<User, object>>[] {u => u.Role});
        }
        public async Task<User?> GetUser(int id)
        {
            return await _genericRepository.FirstOrDefaultAsync(u => u.Id == id,
                    new System.Linq.Expressions.Expression<Func<User, object>>[] { u => u.Role });
        }

        public async Task AddUser(User user)
        {
            await _genericRepository.CreateAsync(user);
        }
        public async Task UpdateUser(User user)
        {
            await _genericRepository.UpdateAsync(user);
        }
        public async Task<IList<User>> GetUsers()
        {
            var list = await _genericRepository.WhereAsync(a => a.RoleId != 4,
               new System.Linq.Expressions.Expression<Func<User, object>>[] { a=> a.Role});
            return (list.Count() > 0) ? list : null;
        }
    }
}
