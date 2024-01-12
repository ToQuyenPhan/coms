using Coms.Application.Services.Common;
using ErrorOr;

namespace Coms.Application.Services.ContractAnnexes
{
    public interface IContractAnnexesService
    {
        //get contractannexes by contractId
        Task<ErrorOr<PagingResult<ContractAnnexesResult>>> GetContractAnnexesByContractId(int contractId, string name, int? status, int currentPage, int pageSize);
        //get contractannexes by contractAnnexId
        Task<ErrorOr<ContractAnnexesResult>> GetContractAnnexesById(int id);
        //delete contractannexes by contractAnnexId
        Task<ErrorOr<ContractAnnexesResult>> DeleteContractAnnex(int id);
        //get your contractannexes
        Task<ErrorOr<PagingResult<ContractAnnexesResult>>> GetYourContractAnnexes(int userId, string name, int? status, bool isYour, int currentPage, int pageSize);
        //get manager contractannexes
        Task<ErrorOr<PagingResult<ContractAnnexesResult>>> GetManagerContractAnnexes(int userId, string name, int status, int currentPage, int pageSize);
        //approve or reject contractannexes by contractAnnexId
        Task<ErrorOr<ContractAnnexesResult>> ApproveContractAnnex(int contractAnnexId, int userId, bool isApproved);
        //manage sign contractannexes by contractAnnexId
        //Task<ErrorOr<ContractAnnexesResult>> ManagerSignContractAnnex(int id, int status);
        //get isAuthor
        Task<ErrorOr<AuthorResult>> IsAuthor(int userId, int contractAnnexId);
    }
}
