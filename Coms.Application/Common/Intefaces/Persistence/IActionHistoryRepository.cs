using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IActionHistoryRepository
    {
        Task<IList<ActionHistory>> GetCreateActionByUserId(int userId);
        Task<IList<ActionHistory>> GetCommentActionByContractId(int contractId, int userId);
    }
}
