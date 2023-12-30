using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IContractCostRepository
    {
        Task<ContractCost> GetContractCost(int id);
        Task<ContractCost?> GetByContractId(int contractId);
        Task AddContractCost(ContractCost contractCost);
        Task AddContractCostsToContract(int[] services, int contractId);
    }
}
