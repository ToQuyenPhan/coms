using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsername(string username);
        void Add(User user);
    }
}
