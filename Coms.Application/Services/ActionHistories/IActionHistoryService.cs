using Coms.Application.Services.Common;
using ErrorOr;

namespace Coms.Application.Services.ActionHistories
{
    public interface IActionHistoryService
    {
        Task<ErrorOr<PagingResult<ActionHistoryResult>>> GetRecentActivities(int userId, int currentPage, int pageSize);
    }
}
