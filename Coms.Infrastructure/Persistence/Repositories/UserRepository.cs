using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;

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
        public async Task<IList<User>> GetManagers()
        {
            var list = await _genericRepository.WhereAsync(u => u.RoleId == 2 && u.Status == (int)UserStatus.Active,
                   new System.Linq.Expressions.Expression<Func<User, object>>[] { u => u.Role });
            return (list.Count() > 0) ? list : null;
        }

        public IList<User>? GetUsers()
        {
            var list = _genericRepository.GetAllWithIncludes(new System.Linq.Expressions.Expression<Func<User, object>>[] { 
                    u => u.Role });
            return (list.Count() > 0) ? list : null;
        }

        public async Task UpdateUser(User user)
        {
            await _genericRepository.UpdateAsync(user);
        }

        public async Task AddUser(User user)
        {
            await _genericRepository.CreateAsync(user);
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await _genericRepository.FirstOrDefaultAsync(u => u.Email.Equals(email),
                    new System.Linq.Expressions.Expression<Func<User, object>>[] { u => u.Role });
        }

        public async Task<User?> GetByPhone(string phone)
        {
            return await _genericRepository.FirstOrDefaultAsync(u => u.Phone.Equals(phone),
                    new System.Linq.Expressions.Expression<Func<User, object>>[] { u => u.Role });
        }

        public async Task<User?> GetByOtherEmail(string email, int userId)
        {
            return await _genericRepository.FirstOrDefaultAsync(u => u.Email.Equals(email) && u.Id != userId,
                    new System.Linq.Expressions.Expression<Func<User, object>>[] { u => u.Role });
        }

        public async Task<User?> GetByOtherPhone(string phone, int userId)
        {
            return await _genericRepository.FirstOrDefaultAsync(u => u.Phone.Equals(phone) && u.Id != userId,
                    new System.Linq.Expressions.Expression<Func<User, object>>[] { u => u.Role });
        }

        public async Task<User?> GetByOtherUsername(string username, int userId)
        {
            return await _genericRepository.FirstOrDefaultAsync(u => u.Username.Equals(username) && u.Id != userId,
                    new System.Linq.Expressions.Expression<Func<User, object>>[] { u => u.Role });
        }

        public async Task EditUser(User user)
        {
            await _genericRepository.UpdateAsync(user);
        }
        public async Task<IList<User>> GetUsers()
        {
            var list = await _genericRepository.WhereAsync( u => u.Status == (int)UserStatus.Active,
                   new System.Linq.Expressions.Expression<Func<User, object>>[] { u => u.Role });
            return (list.Count() > 0) ? list : null;
        }

        //public async Task<IList<User>> GetManagers()
        //{
        //    var list = await _genericRepository.WhereAsync(a => a.RoleId == 2
        //        && a.Status == (int)UserStatus.Active,
        //       new System.Linq.Expressions.Expression<Func<User, object>>[] {
        //            a => a.UserAccesses,a=> a.Templates,a=>a.ActionHistories, a=> a.Role});
        //    return (list.Count() > 0) ? list : null;
        //}

        //public async Task<IList<User>> GetStaffs()
        //{
        //    var list = await _genericRepository.WhereAsync(a => a.RoleId == 1
        //        && a.Status == (int)UserStatus.Active,
        //       new System.Linq.Expressions.Expression<Func<User, object>>[] {
        //            a => a.UserAccesses,a=> a.Templates,a=>a.ActionHistories, a=> a.Role});
        //    return (list.Count() > 0) ? list : null;
        //}
    }
}
