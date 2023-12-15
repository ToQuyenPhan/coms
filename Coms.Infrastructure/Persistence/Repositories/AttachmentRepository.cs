using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class AttachmentRepository: IAttachmentRepository
    {
        private readonly IGenericRepository<Attachment> _genericRepository;

        public AttachmentRepository(IGenericRepository<Attachment> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<IList<Attachment>> GetAttachmentsByContractId(int contractId)
        {
            var list = await _genericRepository.WhereAsync(a => a.ContractId.Equals(contractId) && 
                a.Status != Domain.Enum.AttachmentStatus.Inactive, null);
            return (list.Count() > 0) ? list : null;
        }
    }
    
}
