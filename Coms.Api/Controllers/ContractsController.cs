using Coms.Application.Services.Common;
using Coms.Application.Services.Contracts;
using Coms.Contracts.Contracts;
using Coms.Domain.Entities;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Staff")]
    public class ContractsController : ApiController
    {
        private readonly IContractService _contractService;

        public ContractsController(IContractService contractService)
        {
            _contractService = contractService;
        }

        [HttpGet("yours")]
        [SwaggerOperation(Summary = "Get your contracts in Coms")]
        public IActionResult GetYourContracts([FromQuery] YourContractsFilterRequest request)
        {
            ErrorOr<PagingResult<ContractResult>> result = _contractService.GetYourContracts(
                int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value),
                request.ContractName, request.CreatorName, request.Status, request.CurrentPage, 
                request.PageSize).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "Get your contracts in Coms")]
        public IActionResult Delete([FromQuery] int id)
        {
            ErrorOr<ContractResult> result = _contractService.DeleteContract(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add a contract in Coms")]
        public IActionResult Add(ContractFormRequest request)
        {
            ErrorOr<ContractResult> result =
                _contractService.AddContract(request.ContractName,request.Code,request.PartnerId, int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value)
                , request.TemplateId, request.EffectiveDate, request.Link, request.Status).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
      
    }
}
