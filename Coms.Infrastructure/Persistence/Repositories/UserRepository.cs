using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using Coms.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ComsDBContext _dbContext;

        public UserRepository(ComsDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly List<User> _users = new();
        public void Add(User user)
        {
            _users.Add(user);
        }

        public User? GetUserByUsername(string username)
        {
            _users.Add(new User { Username = username, Password = "string" });
            return _users.SingleOrDefault(u => u.Username == username);
        }
    }
}
