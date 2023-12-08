using Coms.Application.Services.Common;
using ErrorOr;

namespace Coms.Application.Services.ContractAnnexes
{
    public interface IContractAnnexesService
    {
        //get all contractannexes
        Task<ErrorOr<PagingResult<ContractAnnexesResult>>> GetContractAnnexes(string name, int? status, int currentPage, int pageSize);
        //get contractannexes by contractId
        Task<ErrorOr<PagingResult<ContractAnnexesResult>>> GetContractAnnexesByContractId(int contractId, string name, int? status, int currentPage, int pageSize);
        //get contractannexes by contractAnnexId
        Task<ErrorOr<ContractAnnexesResult>> GetContractAnnexesById(int id);
        //delete contractannexes by contractAnnexId
        Task<ErrorOr<ContractAnnexesResult>> DeleteContractAnnex(int id);
    }
}
