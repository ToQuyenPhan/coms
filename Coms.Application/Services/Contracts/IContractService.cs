﻿using Coms.Application.Services.Common;
using Coms.Domain.Entities;
using ErrorOr;

namespace Coms.Application.Services.Contracts
{
    public interface IContractService
    {
        Task<ErrorOr<ContractResult>> DeleteContract(int id);
        Task<ErrorOr<PagingResult<ContractResult>>> GetYourContracts(int userId,
                string name, string creatorName, int? status, int currentPage, int pageSize);
        Task<ErrorOr<ContractResult>> AddContract(string contractName, string code, int partnerId, int auhtorId, int templateId, DateTime effectiveDate,
                string link, int[] service);
        Task<ErrorOr<IList<GeneralReportResult>>> GetGeneralReport(int userId);
        //add get contract by id
        Task<ErrorOr<ContractResult>> GetContract(int id);
    }
}
