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
    }
}
