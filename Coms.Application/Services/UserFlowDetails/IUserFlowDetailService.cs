using Coms.Application.Services.Common;
using ErrorOr;

namespace Coms.Application.Services.UserFlowDetails
{
    public interface IUserFlowDetailService
    {
        Task<ErrorOr<PagingResult<UserFlowDetailResult>>> GetContractFlowDetails(int contractId, int currentPage,
                int pageSize);
        Task<ErrorOr<PagingResult<NotificationResult>>> GetNotifications(int userId, int currentPage, int pageSize);
        Task<ErrorOr<UserFlowDetailResult>> AddContractFlowDetail(int status, int flowDetailId, int contractId, int liquidationRecordId, int contractAnnexId);
        Task<ErrorOr<PagingResult<UserFlowDetailResult>>> GetContractAnnexFlowDetails(int contractAnnexId,
                           int currentPage, int pageSize);
    }
}
