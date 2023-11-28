using Coms.Application.Services.Common;
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
                 int[] service);
        Task<ErrorOr<IList<GeneralReportResult>>> GetGeneralReport(int userId);
        Task<ErrorOr<ContractResult>> GetContract(int id);
        Task<ErrorOr<PagingResult<ContractResult>>> GetManagerContracts(int userId,
                string name, string creatorName, int? status, int currentPage, int pageSize);
        Task<ErrorOr<PagingResult<ContractResult>>> GetContractForPartner(int partnerId,
                string name, string code, bool isApproved, int currentPage, int pageSize);
        Task<ErrorOr<ContractResult>> ApproveContract(int contractId, int userId, bool isApproved);
    }
}
