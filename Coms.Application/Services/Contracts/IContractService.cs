using Coms.Application.Services.Common;
using ErrorOr;

namespace Coms.Application.Services.Contracts
{
    public interface IContractService
    {
        Task<ErrorOr<ContractResult>> DeleteContract(int userId, int id);
        Task<ErrorOr<PagingResult<ContractResult>>> GetYourContracts(int userId,
                string name, string code, int? version, int? status, bool isYours, int currentPage, int pageSize);
        Task<ErrorOr<int>> AddContract(string[] names, string[] values, int contractCategoryId,
                int serviceId, DateTime effectiveDate, int status, int userId, int partnerId, int templatetype,
                DateTime approveDate, DateTime signDate);
        Task<ErrorOr<IList<GeneralReportResult>>> GetGeneralReport(int userId);
        Task<ErrorOr<ContractResult>> GetContract(int id);
        Task<ErrorOr<PagingResult<ContractResult>>> GetManagerContracts(int userId,
                string name, string code, string partnerName, int? version, int? status, int currentPage, int pageSize);
        Task<ErrorOr<PagingResult<ContractResult>>> GetContractForPartner(int partnerId,
                string name, string code, int? version,int documentStatus, bool isApproved, int currentPage, int pageSize);
        Task<ErrorOr<ContractResult>> ApproveContract(int contractId, int userId, bool isApproved);
        Task<ErrorOr<AuthorResult>> IsAuthor(int userId, int contractId);
        Task<ErrorOr<string>> UploadContract(int contractId);
        Task<ErrorOr<MemoryStream>> PreviewContract(string[] names, string[] values, int contractCategoryId, int templateType);
        Task<ErrorOr<PagingResult<ContractResult>>> GetManagerContractsForSign(int userId,
                string name, string creatorName, int? status, int currentPage, int pageSize);
        Task<ErrorOr<ContractResult>> GetPartnerAndService(int id);
        Task<ErrorOr<int>> EditContract(int contractId, string[] names, string[] values, int serviceId,
                DateTime effectiveDate, int status, int userId, int partnerId, DateTime approveDate, DateTime signDate);
        Task<ErrorOr<PagingResult<ContractResult>>> GetContractsByServiceOrPartner(
                string name, string code, int? status, int? serviceId, int? partnerId, DateTime? startDate, DateTime? endDate, int currentPage, int pageSize);
        Task<ErrorOr<IList<GeneralReportResult>>> GetGeneralReport();
        Task<ErrorOr<ContractResult>> RejectContract(int contractId, bool isApproved);
    }
}
