using Coms.Domain.Entities;
using Coms.Domain.Enum;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IContractRepository
    {
        Task<Contract?> GetContract(int id);
        Task UpdateContract(Contract contract);
        Task<IList<Contract>> GetContractsByStatus(DocumentStatus status);
        Task AddContract(Contract contract);
        Task<IList<Contract>?> GetByContractCode(string code);
        Task<IList<Contract>> GetContracts();
    }
}
