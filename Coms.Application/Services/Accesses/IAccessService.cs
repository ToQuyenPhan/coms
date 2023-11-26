using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.Accesses
{
    public interface IAccessService
    {
        Task<ErrorOr<AccessResult>> AddAccess(int contractId, int accessRoleId);
        Task<ErrorOr<AccessResult>> AddViewers(int[] users, int contractId);
    }
}
