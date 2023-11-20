using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IContractRepository
    {
        Task<Contract> GetContract(int id);
        Task UpdateContract(Contract contract);
    }
}
