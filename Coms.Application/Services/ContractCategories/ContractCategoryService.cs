using Coms.Application.Common.Intefaces.Persistence;
using ErrorOr;
using System.Collections.Generic;

namespace Coms.Application.Services.ContractCategories
{
    public class ContractCategoryService : IContractCategoryService
    {
        public readonly IContractCategoryRepository _contractCategoryRepository;

        public ContractCategoryService(IContractCategoryRepository contractCategoryRepository)
        {
            _contractCategoryRepository = contractCategoryRepository;
        }

        public ErrorOr<IList<ContractCategoryResult>> GetAllActiveContractCategories()
        {
            if(_contractCategoryRepository.GetActiveContractCategories().Result is not null)
            {
                IList<ContractCategoryResult> responses = new List<ContractCategoryResult>();
                var results = _contractCategoryRepository.GetActiveContractCategories().Result;
                foreach (var category in results)
                {
                    var response = new ContractCategoryResult
                    {
                        Id = category.Id,
                        CategoryName = category.CategoryName,
                        Status = category.Status,
                    };
                    responses.Add(response);
                }
                return responses.ToList();
            }
            else
            {
                return new List<ContractCategoryResult>();
            }
        }
        //add get contract category by id
        public ErrorOr<ContractCategoryResult> GetContractCategoryById(int id)
        {
            if (_contractCategoryRepository.GetActiveContractCategoryById(id).Result is not null)
            {
                var result = _contractCategoryRepository.GetActiveContractCategoryById(id).Result;
                var response = new ContractCategoryResult
                {
                    Id = result.Id,
                    CategoryName = result.CategoryName,
                    Status = result.Status,
                };
                return response;
            }
            else
            {
                return new ContractCategoryResult();
            }
        }
    }
}
