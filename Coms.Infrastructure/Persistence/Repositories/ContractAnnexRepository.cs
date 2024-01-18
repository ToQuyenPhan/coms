using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class ContractAnnexRepository : IContractAnnexRepository
    {
        private readonly IGenericRepository<ContractAnnex> _genericRepository;

        public ContractAnnexRepository(IGenericRepository<ContractAnnex> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        //get all ContractAnnexes
        public async Task<IList<ContractAnnex>?> GetContractAnnexes()
        {
            IList<ContractAnnex> list = await _genericRepository.WhereAsync(c => c.Id > 0, null);
            return (list.Count() > 0) ? list : null;
        }

        //get ContractAnnexes by contractId
        public async Task<IList<ContractAnnex>?> GetContractAnnexesByContractId(int contractId)
        {
            IList<ContractAnnex> list = await _genericRepository.WhereAsync(c => c.ContractId.Equals(contractId), null);
            return (list.Count() > 0) ? list : null;
        }

        //get contractannexes by contractAnnexId
        public async Task<ContractAnnex> GetContractAnnexesById(int contractAnnexId)
        {
            return await _genericRepository.FirstOrDefaultAsync(c => c.Id.Equals(contractAnnexId), null);
        }

        //Update contractannexes by ContractAnnex
        public async Task UpdateContractAnnexes(ContractAnnex contractAnnex)
        {
            await _genericRepository.UpdateAsync(contractAnnex);
        }
        //get contractannex by contract annex code
        public async Task<ContractAnnex> GetByContractAnnexCode(string contractAnnexCode)
        {
            return await _genericRepository.FirstOrDefaultAsync(c => c.Code.Equals(contractAnnexCode), null);
        }
        //AddContractAnnex
        public async Task AddContractAnnex(ContractAnnex contractAnnex)
        {
            await _genericRepository.CreateAsync(contractAnnex);
        }
        //get contractannex by contract annex code2
        public async Task<IList<ContractAnnex>?> GetByContractAnnexCode2(string code)
        {
            var list = await _genericRepository.WhereAsync(c => c.Code.Contains(code), null);
            return (list.Count() > 0) ? list : null;
        }


    }
}
