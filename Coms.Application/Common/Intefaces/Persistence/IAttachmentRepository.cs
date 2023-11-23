using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IAttachmentRepository
    {
        //add get all attachments by contract id
        Task<IList<Attachment>> GetAttachmentsByContractId(int contractId);
    }
}
