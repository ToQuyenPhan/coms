using Coms.Application.Services.Authentication;
using Coms.Application.Services.FlowDetails;
using Coms.Domain.Enum;
using ErrorOr;

namespace Coms.Application.Services.ContractCategories
{
    public interface IContractCategoryService
    {
        ErrorOr<IList<ContractCategoryResult>> GetAllActiveContractCategories();
        //add get contract category by id
        ErrorOr<ContractCategoryResult> GetContractCategoryById(int id);
        Task<ErrorOr<ContractCategoryResult>> CreateContractCategory(string categoryName, ContractCategoryStatus status);
    }
}
