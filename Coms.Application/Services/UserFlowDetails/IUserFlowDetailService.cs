using Coms.Application.Services.Common;
using ErrorOr;

namespace Coms.Application.Services.UserFlowDetails
{
    public interface IUserFlowDetailService
    {
        Task<ErrorOr<PagingResult<UserFlowDetailResult>>> GetContractFlowDetails(int contractId, int currentPage,
                int pageSize);
    }
}
