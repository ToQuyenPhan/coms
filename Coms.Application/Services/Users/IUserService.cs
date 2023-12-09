using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.Users
{
    public interface IUserService
    {
        Task<ErrorOr<IList<UserResult>>> GetUsers();
        Task<ErrorOr<IList<UserResult>>> GetManagers();
        Task<ErrorOr<IList<UserResult>>> GetStaffs(int userId);
        Task<ErrorOr<UserResult>> GetUser(int id);
        Task<ErrorOr<UserResult>> AddUser(string fullName, string username, string email, string password, DateTime dob, string image, int roleId, int status);
        Task<ErrorOr<UserResult>> UpdateUser(string fullName, string username, string email, string password, DateTime dob, string image, int roleId, int status, int userId);
        Task<ErrorOr<UserResult>> DeleteUser(int userId);
    }
}
