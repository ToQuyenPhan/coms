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
    public class LiquidationRecordFileRepository : ILiquidationRecordFileRepository
    {
        private readonly IGenericRepository<LiquidationRecordFile> _genericRepository;

        public LiquidationRecordFileRepository(IGenericRepository<LiquidationRecordFile> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task Add(LiquidationRecordFile liquidationRecordFile)
        {
            await _genericRepository.CreateAsync(liquidationRecordFile);
        }

        public async Task<LiquidationRecordFile?> GetLiquidationRecordFileByContractId(int liquidationRecordId)
        {
            return await _genericRepository
                .FirstOrDefaultAsync(tf => tf.LiquidationRecordId.Equals(liquidationRecordId));
        }

        public async Task<LiquidationRecordFile?> GetLiquidationRecordFileById(Guid id)
        {
            return await _genericRepository
                .FirstOrDefaultAsync(tf => tf.UUID.Equals(id));
        }

        public async Task Update(LiquidationRecordFile liquidationRecordFile)
        {
            await _genericRepository.UpdateAsync(liquidationRecordFile);
        }

    }
}
