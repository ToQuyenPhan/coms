using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using Coms.Domain.Enum;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class FlowRepository : IFlowRepository
    {
        private readonly IGenericRepository<Flow> _genericRepository;

        public FlowRepository(IGenericRepository<Flow> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<Flow?> GetByContractCategoryId(int contractCategoryId)
        {
            return await 
                    _genericRepository.FirstOrDefaultAsync(f => 
                    f.ContractCategoryId.Equals(contractCategoryId) && 
                    f.Status.Equals(CommonStatus.Active));
        }
    }
}
