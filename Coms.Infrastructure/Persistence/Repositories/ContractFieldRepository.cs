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
    }
}
