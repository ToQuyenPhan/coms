using Coms.Application.Services.Common;
using ErrorOr;

namespace Coms.Application.Services.ActionHistories
{
    public interface IActionHistoryService
    {
        Task<ErrorOr<ActionHistoryResult>> AddActionHistory(int userId, int contractId, int actionType);

        Task<ErrorOr<PagingResult<ActionHistoryResult>>> GetRecentActivities(int userId, int currentPage, int pageSize);
        Task<ErrorOr<MemoryStream>> ExportActionHistories(int userId);

        //add get action history by contract id
        Task<ErrorOr<IList<ActionHistoryResult>>> GetActionHistoryByContractId(int contractId);
    }
}
