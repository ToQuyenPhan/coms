using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface ILiquidationRecordAttachmentRepository
    {
        Task<IList<LiquidationRecordAttachment>> GetAttachmentsByContractId(int liquidationRecordId);
        Task UpdateLiquidationRecordAttachment(LiquidationRecordAttachment attachment);
        Task<LiquidationRecordAttachment?> GetLiquidationRecordAttachment(int id);
        Task AddLiquidationRecordAttachment(LiquidationRecordAttachment attachment);
    }
}
