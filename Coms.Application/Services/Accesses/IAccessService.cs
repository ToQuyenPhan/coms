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
        Task<ErrorOr<IList<AccessResult>>> AddViewers(int[] users, int contractId);
        Task<ErrorOr<IList<AccessResult>>> AddApproves(int[] users, int contractId);
    }
}
