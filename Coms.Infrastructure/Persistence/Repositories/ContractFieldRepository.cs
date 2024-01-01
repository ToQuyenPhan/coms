using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class ContractFieldRepository : IContractFieldRepository
    {
        private readonly IGenericRepository<ContractField> _genericRepository;

        public ContractFieldRepository(IGenericRepository<ContractField> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task AddRangeContractField(List<ContractField> contractFields)
        {
            await _genericRepository.CreateRangeAsync(contractFields);
        }

        public async Task<IList<ContractField>?> GetByContractId(int contractId)
        {
            var list = await _genericRepository.WhereAsync(cf => cf.ContractId.Equals(contractId), null);
            return (list.Count() > 0) ? list : null;
        }
    }
}
