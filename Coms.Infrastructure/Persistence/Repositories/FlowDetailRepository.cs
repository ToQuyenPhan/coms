using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class FlowDetailRepository : IFlowDetailRepository
    {
        private readonly IGenericRepository<FlowDetail> _genericRepository;

        public FlowDetailRepository(IGenericRepository<FlowDetail> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<FlowDetail?> GetFlowDetail(int id)
        {
            return await _genericRepository.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
