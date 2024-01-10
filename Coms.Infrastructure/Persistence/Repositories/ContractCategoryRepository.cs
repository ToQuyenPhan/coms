using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using Coms.Domain.Enum;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class ContractCategoryRepository : IContractCategoryRepository
    {
        private readonly IGenericRepository<ContractCategory> _genericRepository;

        public ContractCategoryRepository(IGenericRepository<ContractCategory> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task CreateContractCategory(ContractCategory contractCategory)
        {
            await _genericRepository.CreateAsync(contractCategory);
        }
        public async Task<ContractCategory?> GetCategoryByName(string categoryName)
        {
            return await _genericRepository.FirstOrDefaultAsync(c => c.CategoryName.Equals(categoryName), null);
        }

        public async Task<IList<ContractCategory>?> GetActiveContractCategories()
        {
            var list = await _genericRepository.WhereAsync(
                    cc => cc.Status.Equals(ContractCategoryStatus.Active), null);
            return (list.Count > 0) ? list : null;
        }

        public async Task<ContractCategory?> GetActiveContractCategoryById(int id)
        {
            var item = await _genericRepository.FirstOrDefaultAsync(cc => cc.Id == id && 
                cc.Status.Equals(ContractCategoryStatus.Active));
            return item;
        }
    }
}
