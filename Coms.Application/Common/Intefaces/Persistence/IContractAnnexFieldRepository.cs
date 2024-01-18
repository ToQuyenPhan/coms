using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IContractAnnexFieldRepository
    {
        Task AddRangeContractAnnexField(List<ContractAnnexField> contractFields);
        Task<IList<ContractAnnexField>?> GetByContractAnnexId(int contractAnnexId);
        //Add
        Task Add(ContractAnnexField contractAnnexField);
    }
}
