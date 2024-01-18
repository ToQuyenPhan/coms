using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.FlowDetails;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
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

        public async Task<ErrorOr<ContractCategoryResult>> CreateContractCategory(string categoryName, ContractCategoryStatus status)
        {
            try
            {
                var isExist = await _contractCategoryRepository.GetCategoryByName(categoryName);
                if (isExist is not null)
                {
                    return Error.NotFound("Category name already exist!");
                }
                var contractCate = new ContractCategory
                {
                    CategoryName = categoryName,
                    Status = status
                };

                await _contractCategoryRepository.CreateContractCategory(contractCate);
                var result = new ContractCategoryResult
                {
                    Id = contractCate.Id,
                    CategoryName = contractCate.CategoryName,
                    Status = contractCate.Status
                };

                return result;
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
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
        public async Task<ErrorOr<ContractCategoryResult>> DeleteContractCategoryById(int id)
        {
            if (_contractCategoryRepository.GetActiveContractCategoryById(id).Result is not null)
            {
                var result = _contractCategoryRepository.GetActiveContractCategoryById(id).Result;
                result.Status = ContractCategoryStatus.Inactive;
                await _contractCategoryRepository.UpdateContractCategory(result);
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
