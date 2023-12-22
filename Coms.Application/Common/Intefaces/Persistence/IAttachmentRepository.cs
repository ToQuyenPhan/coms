using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IAttachmentRepository
    {
        Task<IList<Attachment>> GetAttachmentsByContractId(int contractId);
        Task UpdateAttachment(Attachment attachment);
        Task<Attachment?> GetAttachment(int id);
        Task AddAttachment(Attachment attachment);
    }
}
