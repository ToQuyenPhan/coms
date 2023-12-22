using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly IGenericRepository<Attachment> _genericRepository;

        public AttachmentRepository(IGenericRepository<Attachment> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<IList<Attachment>?> GetAttachmentsByContractId(int contractId)
        {
            IList<Attachment> list = await _genericRepository.WhereAsync(a => a.ContractId.Equals(contractId) &&
                a.Status != Domain.Enum.AttachmentStatus.Inactive, null);
            return (list.Count() > 0) ? list : null;
        }

        public async Task UpdateAttachment(Attachment attachment)
        {
            await _genericRepository.UpdateAsync(attachment);
        }

        public async Task<Attachment?> GetAttachment(int id)
        {
            return await _genericRepository.FirstOrDefaultAsync(a => a.Id.Equals(id));
        }

        public async Task AddAttachment(Attachment attachment)
        {
            await _genericRepository.CreateAsync(attachment);
        }
    }
}
