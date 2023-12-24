using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IPartnerCommentRepository
    {
        Task<IList<PartnerComment>?> GetByPartnerReviewId(int partnerReviewId);
    }
}
