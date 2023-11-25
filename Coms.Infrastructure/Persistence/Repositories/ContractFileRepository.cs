using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class ContractFileRepository : IContractFileRepository
    {
        private readonly IGenericRepository<ContractFile> _genericRepository;

        public ContractFileRepository(IGenericRepository<ContractFile> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task Add(ContractFile contractFile)
        {
            await _genericRepository.CreateAsync(contractFile);
        }

        public async Task<ContractFile?> GetContractFileByContractId(int contractId)
        {
            return await _genericRepository
                .FirstOrDefaultAsync(tf => tf.ContractId.Equals(contractId));
        }
    }
}
