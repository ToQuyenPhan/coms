using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IActionHistoryRepository
    {
        Task<IList<ActionHistory>?> GetCreateActionByUserId(int userId);
        Task<IList<ActionHistory>> GetCommentActionByContractId(int contractId, int userId);
        Task<IList<ActionHistory>> GetOtherUserActionByContractId(int contractId, int userId);
        Task<ActionHistory?> GetCreateActionByContractId(int contractId);
        Task<ActionHistory> GetActionHistoryById(int id);
        Task AddActionHistory(ActionHistory actionHistory);
        //add get comment action by contract id
        Task<IList<ActionHistory>> GetCommentActionByContractId(int contractId);
    }
}
