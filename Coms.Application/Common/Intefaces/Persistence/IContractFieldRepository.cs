using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IContractFieldRepository
    {
        Task AddRangeContractField(List<ContractField> contractFields);
    }
}
