using Coms.Domain.Entities;


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
