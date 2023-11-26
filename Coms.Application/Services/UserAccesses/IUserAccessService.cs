using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.UserAccesses
{
    public interface IUserAccessService
    {
        Task<ErrorOr<UserAccessResult>> AddUserAccess(int contractId, int userId, int accessRole);
    }
}
