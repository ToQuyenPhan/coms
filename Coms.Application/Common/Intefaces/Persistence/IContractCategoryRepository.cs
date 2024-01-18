using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IContractCategoryRepository
    {
        Task<IList<ContractCategory>?> GetActiveContractCategories();
        Task<ContractCategory?> GetActiveContractCategoryById(int id);
        Task CreateContractCategory(ContractCategory contractCategory);
        Task<ContractCategory?> GetCategoryByName(string categoryName);
        Task UpdateContractCategory(ContractCategory contractCategory);
    }
}
