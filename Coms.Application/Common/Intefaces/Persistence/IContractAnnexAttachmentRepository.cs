using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IContractAnnexAttachmentRepository
    {
        Task<IList<ContractAnnexAttachment>> GetAttachmentsByContractAnnexId(int contractAnnexId);
        Task UpdateAttachment(ContractAnnexAttachment attachment);
        Task<ContractAnnexAttachment?> GetAttachment(int id);
        Task AddAttachment(ContractAnnexAttachment attachment);
    }
}
