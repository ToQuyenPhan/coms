using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsername(string username);
        Task<User?> GetUser(int id);
        IList<User>? GetUsers();
        Task UpdateUser(User user);
        Task AddUser(User user);
        Task<User?> GetByEmail(string email);
        Task<User?> GetByPhone(string phone);
        //Task<IList<User>> GetManagers();
        //Task<IList<User>> GetStaffs();
    }
}
