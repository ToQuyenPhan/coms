using Coms.Application.Services.ContractCategories;
using Coms.Application.Services.FlowDetails;
using Coms.Application.Services.LiquidationRecords;
using Coms.Contracts.ContractCategories;
using Coms.Contracts.FlowDetails;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

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
        [Authorize(Roles = "Sale Manager")]
        [HttpPost("add")]
        [SwaggerOperation(Summary = "Create a contract category in Coms")]
        public IActionResult CreateCategory([FromBody] ContractCategoryFormRequest request)
        {
            ErrorOr<ContractCategoryResult> result =
                _contractCategoryService.CreateContractCategory(request.ContractCategoryName, request.Status).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
        [HttpDelete("id")]
        [SwaggerOperation(summary: "Delete Contract category by Id in Coms")]
        [Authorize(Roles = "Sale Manager")]
        public IActionResult DeleteContractCategory([FromQuery][Required] int id)
        {
            try
            {
                ErrorOr<ContractCategoryResult> result = _contractCategoryService.DeleteContractCategoryById(id).Result;
                return result.Match(
                    result => Ok(result),
                    errors => Problem(errors)
                );
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }

        }
    }
}
