using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Application.Services.Templates;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;
using LinqKit;
using System.Diagnostics.Contracts;

namespace Coms.Application.Services.LiquidationRecords
{
    public class LiquidationRecordsService : ILiquidationRecordsService
    {
        private readonly ILiquidationRecordRepository _liquidationRecordRepository;

        public LiquidationRecordsService(ILiquidationRecordRepository liquidationRecordRepository)
        {
            _liquidationRecordRepository = liquidationRecordRepository;
        }

        //get all LiquidationRecords
        public async Task<ErrorOr<PagingResult<LiquidationRecordsResult>>> GetLiquidationRecords(string name, int? status, int currentPage, int pageSize)
        {
            var predicate = PredicateBuilder.New<LiquidationRecord>(true);
            if (!string.IsNullOrEmpty(name))
            {
                predicate = predicate.And(c => c.LiquidationName.Contains(name.Trim()));
            }
            if (status is not null)
            {
                if (status >= 0)
                {
                    predicate = predicate.And(c => c.Status.Equals((DocumentStatus)status));
                }
            }
            var liquidationRecords = await _liquidationRecordRepository.GetLiquidationRecords();
            IList<LiquidationRecord> filteredList = liquidationRecords.Where(predicate).ToList();
            var total = filteredList.Count();
            if (currentPage > 0 && pageSize > 0)
            {
                filteredList = filteredList.Skip((currentPage - 1) * pageSize).Take(pageSize)
                        .ToList();
            }
            IList<LiquidationRecordsResult> responses = new List<LiquidationRecordsResult>();
            foreach (var liquidationRecord in filteredList)
            {
                var liquidationRecordResult = new LiquidationRecordsResult()
                {
                    Id = liquidationRecord.Id,
                    LiquidationName = liquidationRecord.LiquidationName,
                    Version = liquidationRecord.Version,
                    CreatedDate = liquidationRecord.CreatedDate,
                    CreatedDateString = liquidationRecord.CreatedDate.Date.ToString("dd/MM/yyyy"),
                    UpdatedDate = liquidationRecord.UpdatedDate,
                    UpdatedDateString = liquidationRecord.UpdatedDate.ToString(),
                    Status = liquidationRecord.Status,
                    StatusString = liquidationRecord.Status.ToString(),
                    ContractId = liquidationRecord.ContractId,
                    Link = liquidationRecord.Link
                };
                responses.Add(liquidationRecordResult);
            }
            return new
                PagingResult<LiquidationRecordsResult>(responses, total, currentPage, pageSize);
        }

        //get LiquidationRecords by contractId
        public async Task<ErrorOr<PagingResult<LiquidationRecordsResult>>> GetLiquidationRecordsByContractId(int contractId, string name, int? status, int currentPage, int pageSize)
        {
            var predicate = PredicateBuilder.New<LiquidationRecord>(true);
            if (!string.IsNullOrEmpty(name))
            {
                predicate = predicate.And(c => c.LiquidationName.Contains(name.Trim()));
            }
            if (status is not null)
            {
                if (status >= 0)
                {
                    predicate = predicate.And(c => c.Status.Equals((DocumentStatus)status));
                }
            }
            var liquidationRecords = await _liquidationRecordRepository.GetLiquidationRecordsByContractId(contractId);
            IList<LiquidationRecord> filteredList = liquidationRecords.Where(predicate).ToList();
            var total = filteredList.Count();
            if (currentPage > 0 && pageSize > 0)
            {
                filteredList = filteredList.Skip((currentPage - 1) * pageSize).Take(pageSize)
                        .ToList();
            }
            IList<LiquidationRecordsResult> responses = new List<LiquidationRecordsResult>();
            foreach (var liquidationRecord in filteredList)
            {
                var liquidationRecordResult = new LiquidationRecordsResult()
                {
                    Id = liquidationRecord.Id,
                    LiquidationName = liquidationRecord.LiquidationName,
                    Version = liquidationRecord.Version,
                    CreatedDate = liquidationRecord.CreatedDate,
                    CreatedDateString = liquidationRecord.CreatedDate.Date.ToString("dd/MM/yyyy"),
                    UpdatedDate = liquidationRecord.UpdatedDate,
                    UpdatedDateString = liquidationRecord.UpdatedDate.ToString(),
                    Status = liquidationRecord.Status,
                    StatusString = liquidationRecord.Status.ToString(),
                    ContractId = liquidationRecord.ContractId,
                    Link = liquidationRecord.Link
                };
                responses.Add(liquidationRecordResult);
            }
            return new
                PagingResult<LiquidationRecordsResult>(responses, total, currentPage, pageSize);
        }
        //get LiquidationRecords by LiquidationRecordId
        public async Task<ErrorOr<LiquidationRecordsResult>> GetLiquidationRecordsById(int id)
        {
            var liquidationRecord = await _liquidationRecordRepository.GetLiquidationRecordsById(id);
            if (liquidationRecord is not null)
            {
                var liquidationRecordResult = new LiquidationRecordsResult()
                {
                    Id = liquidationRecord.Id,
                    LiquidationName = liquidationRecord.LiquidationName,
                    Version = liquidationRecord.Version,
                    CreatedDate = liquidationRecord.CreatedDate,
                    CreatedDateString = liquidationRecord.CreatedDate.Date.ToString("dd/MM/yyyy"),
                    UpdatedDate = liquidationRecord.UpdatedDate,
                    UpdatedDateString = liquidationRecord.UpdatedDate.ToString(),
                    Status = liquidationRecord.Status,
                    StatusString = liquidationRecord.Status.ToString(),
                    ContractId = liquidationRecord.ContractId,
                    Link = liquidationRecord.Link
                };
                return liquidationRecordResult;
            }
            else
            {
                return Error.NotFound("404", "liquidation Record is not found!");
            }
        }
        public async Task<ErrorOr<LiquidationRecordsResult>> DeleteLiquidationRecords(int id)
        {
            try
            {
                if (_liquidationRecordRepository.GetLiquidationRecordsById(id).Result is not null)
                {
                    var liquidationRecord = await _liquidationRecordRepository.GetLiquidationRecordsById(id);
                    liquidationRecord.Status = DocumentStatus.Deleted;
                    await _liquidationRecordRepository.UpdateLiquidationRecord(liquidationRecord);
                    var liquidationRecordResult = new LiquidationRecordsResult()
                    {
                        Id = liquidationRecord.Id,
                        LiquidationName = liquidationRecord.LiquidationName,
                        Version = liquidationRecord.Version,
                        CreatedDate = liquidationRecord.CreatedDate,
                        CreatedDateString = liquidationRecord.CreatedDate.Date.ToString("dd/MM/yyyy"),
                        UpdatedDate = liquidationRecord.UpdatedDate,
                        UpdatedDateString = liquidationRecord.UpdatedDate.ToString(),
                        Status = liquidationRecord.Status,
                        StatusString = liquidationRecord.Status.ToString(),
                        ContractId = liquidationRecord.ContractId,
                        Link = liquidationRecord.Link
                    };
                    return liquidationRecordResult;
                }
                else
                {
                    return Error.NotFound();
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }
    }
}
