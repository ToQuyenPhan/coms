using Coms.Application.Services.Common;
using ErrorOr;

namespace Coms.Application.Services.Contracts
{
    public interface IContractService
    {
        Task<ErrorOr<PagingResult<ContractResult>>> GetYourContracts(int userId,
                string name, string creatorName, int? status, int currentPage, int pageSize);
    }
}
