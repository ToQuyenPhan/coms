using Coms.Application.Services.Authentication;
using ErrorOr;

namespace Coms.Application.Services.ContractCategories
{
    public interface IContractCategoryService
    {
        ErrorOr<IList<ContractCategoryResult>> GetAllActiveContractCategories();
    }
}
