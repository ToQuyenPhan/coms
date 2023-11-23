using Coms.Application.Services.Accesses;
using Coms.Domain.Enum;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.ActionHistories
{
    public interface IActionHistoryService
    {
        Task<ErrorOr<ActionHistoryResult>> AddActionHistory(int userId, int contractId, int actionType);

    }
}
