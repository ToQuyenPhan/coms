﻿using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using Coms.Domain.Enum;

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
            return await _genericRepository.FirstOrDefaultAsync(c => c.Id.Equals(id), null);
        }

        public async Task UpdateContract(Contract contract)
        {
            await _genericRepository.UpdateAsync(contract);
        }

        public async Task<IList<Contract>> GetContractsByStatus(DocumentStatus status)
        {
            var list = await _genericRepository.WhereAsync(c => c.Status.Equals(status), null);
            return (list.Count() > 0) ? list : null;
        }
    }
}
