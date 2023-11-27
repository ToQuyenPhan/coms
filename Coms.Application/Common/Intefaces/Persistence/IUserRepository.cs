using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsername(string username);
        Task<User?> GetUser(int id);
        Task<IList<User>> GetUsers();
        Task<IList<User>> GetManagers();
    }
}
