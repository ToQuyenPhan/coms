using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IPartnerCommentRepository
    {
        Task<PartnerComment?> GetByPartnerReviewId(int partnerReviewId);
        Task AddPartnerComment(PartnerComment partnerComment);
    }
}
