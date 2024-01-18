using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class ContractAnnexFieldRepository : IContractAnnexFieldRepository
    {
        private readonly IGenericRepository<ContractAnnexField> _genericRepository;

        public ContractAnnexFieldRepository(IGenericRepository<ContractAnnexField> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task AddRangeContractAnnexField(List<ContractAnnexField> contractFields)
        {
            await _genericRepository.CreateRangeAsync(contractFields);
        }

        public async Task<IList<ContractAnnexField>?> GetByContractAnnexId(int contractAnnexId)
        {
            var list = await _genericRepository.WhereAsync(cf => cf.ContractAnnexId.Equals(contractAnnexId), null);
            return (list.Count() > 0) ? list : null;
        }
        //Add
        public async Task Add(ContractAnnexField contractAnnexField)
        {
            await _genericRepository.CreateAsync(contractAnnexField);
        }

    }
}
