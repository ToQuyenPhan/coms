using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class ContractAnnexAttachmentRepository : IContractAnnexAttachmentRepository
    {
        private readonly IGenericRepository<ContractAnnexAttachment> _genericRepository;

        public ContractAnnexAttachmentRepository(IGenericRepository<ContractAnnexAttachment> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        //get attachment by contract annex id
        public async Task<IList<ContractAnnexAttachment>?> GetAttachmentsByContractAnnexId(int contractAnnexId)
        {
            IList<ContractAnnexAttachment> list = await _genericRepository.WhereAsync(a => a.ContractAnnexId.Equals(contractAnnexId) &&
                           a.Status != Domain.Enum.AttachmentStatus.Inactive, null);
            return (list.Count() > 0) ? list : null;
        }
        //update attachment
        public async Task UpdateAttachment(ContractAnnexAttachment attachment)
        {
            await _genericRepository.UpdateAsync(attachment);
        }
        //get attachment by id
        public async Task<ContractAnnexAttachment?> GetAttachment(int id)
        {
            return await _genericRepository.FirstOrDefaultAsync(a => a.Id.Equals(id));
        }
        //add attachment
        public async Task AddAttachment(ContractAnnexAttachment attachment)
        {
            await _genericRepository.CreateAsync(attachment);
        }
    }
}
