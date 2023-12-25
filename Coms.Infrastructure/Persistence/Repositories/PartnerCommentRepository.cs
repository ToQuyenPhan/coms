using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class PartnerCommentRepository : IPartnerCommentRepository
    {
        private readonly IGenericRepository<PartnerComment> _genericRepository;

        public PartnerCommentRepository(IGenericRepository<PartnerComment> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<PartnerComment?> GetByPartnerReviewId(int partnerReviewId)
        {
            return await _genericRepository.FirstOrDefaultAsync(pc => pc.PartnerReviewId.Equals(partnerReviewId), null);
        }

        public async Task AddPartnerComment(PartnerComment partnerComment)
        {
            await _genericRepository.CreateAsync(partnerComment);
        }
    }
}
