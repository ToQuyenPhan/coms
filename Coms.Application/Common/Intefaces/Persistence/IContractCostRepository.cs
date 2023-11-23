using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IContractCostRepository
    {
        Task<ContractCost> GetContractCost(int id);
        Task<IList<ContractCost>> GetContractCostByContractId(int contractId);
        Task AddContractCost(ContractCost contractCost);
        Task AddContractCostsToContract(int[] services, int contractId);
    }
}
