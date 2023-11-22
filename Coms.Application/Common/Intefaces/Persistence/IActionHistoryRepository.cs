using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IActionHistoryRepository
    {
        Task<IList<ActionHistory>> GetCreateActionByUserId(int userId);
        Task<IList<ActionHistory>> GetCommentActionByContractId(int contractId, int userId);
        Task<IList<ActionHistory>> GetOtherUserActionByContractId(int contractId, int userId);
        //add get all comments by contract id
        Task<IList<ActionHistory>> GetCreateActionByContractId(int contractId);
    }
}
