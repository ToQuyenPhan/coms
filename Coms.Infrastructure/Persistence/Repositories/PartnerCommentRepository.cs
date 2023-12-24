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

        public async Task<IList<PartnerComment>?> GetByPartnerReviewId(int partnerReviewId)
        {
            var list = await _genericRepository.WhereAsync(pc => pc.PartnerReviewId.Equals(partnerReviewId), null);
            return (list.Count() > 0 ? list : null);
        }
    }
}
