using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly IGenericRepository<Contract> _genericRepository;

        public ContractRepository(IGenericRepository<Contract> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<Contract> GetContract(int id)
        {
            return await _genericRepository.FirstOrDefaultAsync(c => c.Id.Equals(id),
                new System.Linq.Expressions.Expression<Func<Contract, object>>[]
                    { a => a.Accesses,a=> a.ContractCosts, a=>a.ContractAnnexes, a=> a.ActionHistories});
        }

        public async Task UpdateContract(Contract contract)
        {
            await _genericRepository.UpdateAsync(contract);
        }

        public async Task AddContract(Contract contract)
        {
            await _genericRepository.CreateAsync(contract);
        }
    }
}
