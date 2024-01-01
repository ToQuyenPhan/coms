using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface ILiquidationRecordRepository
    {
        Task<IList<LiquidationRecord>> GetLiquidationRecords();

        Task<IList<LiquidationRecord>> GetLiquidationRecordsByContractId(int contractId);

        Task<LiquidationRecord> GetLiquidationRecordsById(int liquidationRecordId);
        Task UpdateLiquidationRecord(LiquidationRecord liquidationRecord);
    }
}
