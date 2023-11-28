using Coms.Application.Services.ContractCategories;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Sale Manager, Staff, Manager")]
    public class ContractCategoriesController : ApiController
    {
        public readonly IContractCategoryService _contractCategoryService;

        public ContractCategoriesController(IContractCategoryService contractCategoryService)
        {
            _contractCategoryService = contractCategoryService;
        }

        [HttpGet("active")]
        [SwaggerOperation(Summary = "Get all active contract categories of Coms")]
        public IActionResult GetActiveContractCategories()
        {
            ErrorOr<IList<ContractCategoryResult>> result =
                _contractCategoryService.GetAllActiveContractCategories();
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        //add get contract category by id
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get contract category by id in Coms")]
        public IActionResult GetContractCategoryById(int id)
        {
            ErrorOr<ContractCategoryResult> result = _contractCategoryService.GetContractCategoryById(id);
            return result.Match(
                               result => Ok(result),
                                              errors => Problem(errors)
                                                         );
        }
    }
}
