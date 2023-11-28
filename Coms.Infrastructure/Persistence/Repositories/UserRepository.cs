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

        public async Task<IList<User>> GetUsers()
        {
            var list = await _genericRepository.WhereAsync(a=>a.Status == (int)UserStatus.Active
            && a.RoleId != 4,
               new System.Linq.Expressions.Expression<Func<User, object>>[] {
                    a => a.UserAccesses,a=> a.Templates,a=>a.ActionHistories, a=> a.Role});
            return (list.Count() > 0) ? list : null;
        }

        public async Task<IList<User>> GetManagers()
        {
            var list = await _genericRepository.WhereAsync(a => a.RoleId == 2
                && a.Status == (int)UserStatus.Active,
               new System.Linq.Expressions.Expression<Func<User, object>>[] {
                    a => a.UserAccesses,a=> a.Templates,a=>a.ActionHistories, a=> a.Role});
            return (list.Count() > 0) ? list : null;
        }

        public async Task<IList<User>> GetStaffs()
        {
            var list = await _genericRepository.WhereAsync(a => a.RoleId == 1
                && a.Status == (int)UserStatus.Active,
               new System.Linq.Expressions.Expression<Func<User, object>>[] {
                    a => a.UserAccesses,a=> a.Templates,a=>a.ActionHistories, a=> a.Role});
            return (list.Count() > 0) ? list : null;
        }
    }
}
