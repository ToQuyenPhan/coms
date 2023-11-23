using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class AttachmentRepository: IAttachmentRepository
    {
        private readonly IGenericRepository<Attachment> _genericRepository;

        public AttachmentRepository(IGenericRepository<Attachment> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        //add get all attachments by contract id
        public async Task<IList<Attachment>> GetAttachmentsByContractId(int contractId)
        {
            var list = await _genericRepository.WhereAsync(a => a.ContractId.Equals(contractId), null);
            return (list.Count() > 0) ? list : null;
        }
    }
    
}
