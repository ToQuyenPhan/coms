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

        private readonly List<User> _users = new();
        public void Add(User user)
        {
            _users.Add(user);
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _genericRepository.FirstOrDefaultAsync(u => u.Username == username, 
                    new System.Linq.Expressions.Expression<Func<User, object>>[] {u => u.Role});
        }
    }
}
