using Coms.Application.Services.Common;
using ErrorOr;


namespace Coms.Application.Services.LiquidationRecords
{
    public interface ILiquidationRecordsService
    {
        //get all LiquidationRecords
        Task<ErrorOr<PagingResult<LiquidationRecordsResult>>> GetLiquidationRecords(string name, int? status, int currentPage, int pageSize);
        //get LiquidationRecords by contractId
        Task<ErrorOr<PagingResult<LiquidationRecordsResult>>> GetLiquidationRecordsByContractId(int contractId, string name, int? status, int currentPage, int pageSize);
        //get LiquidationRecords by liquidationRecordsId
        Task<ErrorOr<LiquidationRecordsResult>> GetLiquidationRecordsById(int id);
        //delete LiquidationRecords by liquidationRecordsId
        Task<ErrorOr<LiquidationRecordsResult>> DeleteLiquidationRecords(int id);
    }
}

