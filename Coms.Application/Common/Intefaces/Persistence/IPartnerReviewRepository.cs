using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IPartnerReviewRepository
    {
        Task<PartnerReview> GetByContractId(int contractId);
        Task<PartnerReview> GetPartnerReview(int id);
        Task AddPartnerReview(PartnerReview partnerReview);
    }
}
