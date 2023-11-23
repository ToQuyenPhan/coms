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
        Task<ErrorOr<ContractResult>> AddContract(string name, string code, int auhtor,int partnerId, int templateId, DateTime effectiveDate,
                string link, int[] service);
    }
}
