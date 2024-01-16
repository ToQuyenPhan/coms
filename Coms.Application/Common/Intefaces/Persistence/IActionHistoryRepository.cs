using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IActionHistoryRepository
    {
        Task<IList<ActionHistory>?> GetCreateActionByUserId(int userId);
        Task<IList<ActionHistory>?> GetCommentActionByContractId(int contractId, int userId);
        Task<IList<ActionHistory>> GetOtherUserActionByContractId(int contractId, int userId);
        Task<ActionHistory?> GetCreateActionByContractId(int contractId);
        Task<ActionHistory> GetActionHistoryById(int id);
        Task AddActionHistory(ActionHistory actionHistory);
        Task<IList<ActionHistory>?> GetCommentActionByContractId(int contractId);
        Task UpdateActionHistory(ActionHistory actionHistory);
        Task<IList<ActionHistory>?> GetByContractId(int contractId);
        Task<IList<ActionHistory>?> GetCommentActionByContractAnnexId(int contractAnnexId);
        Task<ActionHistory?> GetCreateActionByContractAnnexId(int contractAnnexId);
        Task<IList<ActionHistory>?> GetContractAnnexCreateActionByUserId(int userId);
        Task<IList<ActionHistory>?> GetCreateActions();
    }
}
