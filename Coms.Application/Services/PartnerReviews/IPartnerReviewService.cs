using Coms.Application.Services.Common;
using ErrorOr;

namespace Coms.Application.Services.PartnerReviews
{
    public interface IPartnerReviewService 
    {
        Task<ErrorOr<PartnerReviewResult>> AddPartnerReview(int partnerId,int userId, int contractId);
        Task<ErrorOr<PartnerReviewResult>> ApprovePartnerReview(int contractId, bool isApproved);
        Task<ErrorOr<PagingResult<NotificationResult>>> GetNotifications(int userId, int currentPage, int pageSize);
        Task<ErrorOr<PagingResult<NotificationResult>>> GetPartnerNotifications(int partnerId, int currentPage, int pageSize);
    }
}
