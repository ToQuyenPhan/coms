using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class LiquidationRecordAttachmentRepository : ILiquidationRecordAttachmentRepository
    {
        private readonly IGenericRepository<LiquidationRecordAttachment> _genericRepository;

        public LiquidationRecordAttachmentRepository(IGenericRepository<LiquidationRecordAttachment> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task AddLiquidationRecordAttachment(LiquidationRecordAttachment attachment)
        {
            await _genericRepository.CreateAsync(attachment);
        }

        public async Task<IList<LiquidationRecordAttachment>> GetAttachmentsByContractId(int liquidationRecordId)
        {
            IList<LiquidationRecordAttachment> list = await _genericRepository.WhereAsync(a => a.LiquidationRecordId.Equals(liquidationRecordId) &&
               a.Status != Domain.Enum.AttachmentStatus.Inactive, null);
            return (list.Count() > 0) ? list : null;
        }

        public async Task<LiquidationRecordAttachment?> GetLiquidationRecordAttachment(int id)
        {
            return await _genericRepository.FirstOrDefaultAsync(a => a.Id.Equals(id));
        }

        public async Task UpdateLiquidationRecordAttachment(LiquidationRecordAttachment attachment)
        {
            await _genericRepository.UpdateAsync(attachment);
        }
    }
}
