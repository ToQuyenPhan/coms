using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class UserFlowDetailsRepository : IUserFlowDetailsRepository
    {
        private readonly IGenericRepository<Contract_FlowDetail> _genericRepository;

        public UserFlowDetailsRepository(IGenericRepository<Contract_FlowDetail> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<Contract_FlowDetail?> GetByContractIdAndFlowDetailId(int contractId, int flowDetailId)
        {
            return await _genericRepository.FirstOrDefaultAsync(ufd => ufd.ContractId.Equals(contractId) && 
                ufd.FlowDetailId.Equals(flowDetailId), 
                new System.Linq.Expressions.Expression<Func<Contract_FlowDetail, object>>[] { ufd => ufd.Contract, ufd => ufd.FlowDetail });
        }

        public async Task<IList<Contract_FlowDetail>?> GetByFlowDetailId(int flowDetailId)
        {
            var list =  await _genericRepository.WhereAsync(ufd =>
                ufd.FlowDetailId.Equals(flowDetailId), new System.Linq.Expressions.Expression<Func<Contract_FlowDetail, object>>[] { ufd => ufd.Contract, ufd => ufd.FlowDetail });
            return (list.Count() > 0) ? list : null;
        }

        public async Task<IList<Contract_FlowDetail>?> GetByContractId(int contractId)
        {
            var list = await _genericRepository.WhereAsync(ufd =>
                ufd.ContractId.Equals(contractId), new System.Linq.Expressions.Expression<Func<Contract_FlowDetail, object>>[] { ufd => ufd.Contract, ufd => ufd.FlowDetail });
            return (list.Count() > 0) ? list : null;
        }
    }
}
