using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using Coms.Domain.Enum;

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
            return await _genericRepository.FirstOrDefaultAsync(fd => fd.Id == id);
        }

        public async Task<FlowDetail?> GetSignerByFlowId(int flowId)
        {
            return await _genericRepository.FirstOrDefaultAsync(fd => fd.FlowID == flowId && 
                fd.FlowRole.Equals(FlowRole.Signer));
        }
    }
}
