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
    }
}
