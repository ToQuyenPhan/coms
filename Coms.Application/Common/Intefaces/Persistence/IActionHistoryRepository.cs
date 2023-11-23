using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IActionHistoryRepository
    {
        Task<IList<ActionHistory>> GetCreateActionByUserId(int userId);
        Task<IList<ActionHistory>> GetCommentActionByContractId(int contractId, int userId);
        Task<IList<ActionHistory>> GetOtherUserActionByContractId(int contractId, int userId);
        Task<IList<ActionHistory>> GetCreateActionByContractId(int contractId);
        Task<ActionHistory> GetActionHistoryById(int id);
        Task AddActionHistory(ActionHistory actionHistory);
    }
}
