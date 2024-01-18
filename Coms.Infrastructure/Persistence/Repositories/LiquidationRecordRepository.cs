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
    public class LiquidationRecordRepository : ILiquidationRecordRepository
    {
        private readonly IGenericRepository<LiquidationRecord> _genericRepository;

        public LiquidationRecordRepository(IGenericRepository<LiquidationRecord> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task AddLiquidationrecord(LiquidationRecord liquidationRecord)
        {
            await _genericRepository.CreateAsync(liquidationRecord);
        }

        //get all Liquidation Record
        public async Task<IList<LiquidationRecord>> GetLiquidationRecords()
        {
            var list = await _genericRepository.WhereAsync(c => c.Id > 0, null);
            return (list.Count() > 0) ? list : null;
        }

        //get Liquidation Record by contractId
        public async Task<IList<LiquidationRecord>> GetLiquidationRecordsByContractId(int contractId)
        {
            var list = await _genericRepository.WhereAsync(c => c.ContractId.Equals(contractId), null);
            return (list.Count() > 0) ? list : null;
        }

        //get Liquidation Record by Id
        public async Task<LiquidationRecord> GetLiquidationRecordsById(int liquidationRecordId)
        {
            return await _genericRepository.FirstOrDefaultAsync(c => c.Id.Equals(liquidationRecordId), null);
        }

        public async Task UpdateLiquidationRecord(LiquidationRecord liquidationRecord)
        {
            await _genericRepository.UpdateAsync(liquidationRecord);
        }
    }
}
