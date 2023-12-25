using ErrorOr;

namespace Coms.Application.Services.PartnerReviews
{
    public interface IPartnerReviewService 
    {
        Task<ErrorOr<PartnerReviewResult>> AddPartnerReview(int partnerId,int userId, int contractId);
        Task<ErrorOr<PartnerReviewResult>> ApprovePartnerReview(int contractId, bool isApproved);
    }
}
