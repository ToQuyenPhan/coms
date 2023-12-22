using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IContractFileRepository
    {
        Task Add(ContractFile contractFile);
        Task<ContractFile?> GetContractFileByContractId(int templateId);
    }
}
