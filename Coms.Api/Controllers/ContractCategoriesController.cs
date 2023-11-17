using Coms.Application.Services.ContractCategories;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Sale Manager")]
    public class ContractCategoriesController : ApiController
    {
        public readonly IContractCategoryService _contractCategoryService;

        public ContractCategoriesController(IContractCategoryService contractCategoryService)
        {
            _contractCategoryService = contractCategoryService;
        }

        [HttpGet("active")]
        public IActionResult GetActiveContractCategories()
        {
            ErrorOr<IList<ContractCategoryResult>> result =
                _contractCategoryService.GetAllActiveContractCategories();
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
