using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class ContractAnnexFileRepository : IContractAnnexFileRepository
    {
        private readonly IGenericRepository<ContractAnnexFile> _genericRepository;

        public ContractAnnexFileRepository(IGenericRepository<ContractAnnexFile> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<ContractAnnexFile?> GetContractAnnexFileById(Guid id)
        {
            return await _genericRepository
                .FirstOrDefaultAsync(tf => tf.UUID.Equals(id));
        }

        public async Task Update(ContractAnnexFile contractAnnexFile)
        {
            await _genericRepository.UpdateAsync(contractAnnexFile);
        }
        //Add
        public async Task Add(ContractAnnexFile contractAnnexFile)
        {
            await _genericRepository.CreateAsync(contractAnnexFile);
        }
        public async Task<ContractAnnexFile?> GetContractAnnexFileByContractAnnexId(int contracAnnexId)
        {
            return await _genericRepository
                .FirstOrDefaultAsync(tf => tf.ContractAnnexId.Equals(contracAnnexId));
        }
    }
}
