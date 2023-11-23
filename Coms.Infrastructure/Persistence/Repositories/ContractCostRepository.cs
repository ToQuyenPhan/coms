using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class ContractCostRepository : IContractCostRepository
    {
        private readonly IGenericRepository<ContractCost> _genericRepository;

        public ContractCostRepository(IGenericRepository<ContractCost> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<ContractCost> GetContractCost(int id)
        {
            return await _genericRepository.FirstOrDefaultAsync(a => a.Id.Equals(id),
                    new System.Linq.Expressions.Expression<Func<ContractCost, object>>[]
                    { a => a.Contract, a => a.Service});
        }

        public async Task<IList<ContractCost>> GetContractCostByContractId(int contractId)
        {
            var list = await _genericRepository.WhereAsync(a => a.ContractId.Equals(contractId),
                new System.Linq.Expressions.Expression<Func<ContractCost, object>>[] {
                    a => a.Contract, a=> a.Service });
            return (list.Count() > 0) ? list : null;
        }

        public async Task AddContractCost(ContractCost contractCost)
        {
            await _genericRepository.CreateAsync(contractCost);
        }
        public async Task AddContractCostsToContract(int[] services, int contractId)
        {
            if (services.Any())
            {
                for (int i = 0; i < services.Length; i++)


                {
                    var contractCost = new ContractCost
                    {
                        ContractId = contractId,
                        ServiceId = int.Parse(services[i].ToString()),
                    };
                    await _genericRepository.CreateAsync(contractCost);
                }
            }
        }
    }
}
