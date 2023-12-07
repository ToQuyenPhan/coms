using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using LinqKit;
using System.Linq.Expressions;

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
         public async Task<IList<ContractAnnex>> GetContractAnnexes()
        {
            var list = await _genericRepository.WhereAsync(c => c.Id > 0, null);
            return (list.Count() > 0) ? list : null;
        }

        //get ContractAnnexes by contractId
        public async Task<IList<ContractAnnex>> GetContractAnnexesByContractId(int contractId)
        {
            var list = await _genericRepository.WhereAsync(c => c.ContractId.Equals(contractId), null);
            return (list.Count() > 0) ? list : null;
        }

        //get contractannexes by contractAnnexId
        public async Task<ContractAnnex> GetContractAnnexesById(int contractAnnexId)
        {
            return await _genericRepository.FirstOrDefaultAsync(c => c.Id.Equals(contractAnnexId), null);
        }


        
    }
}
