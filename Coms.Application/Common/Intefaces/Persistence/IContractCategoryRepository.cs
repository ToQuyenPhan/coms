using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IContractCategoryRepository
    {
        Task<IList<ContractCategory>?> GetActiveContractCategories();
        Task<ContractCategory?> GetActiveContractCategoryById(int id);
    }
}
