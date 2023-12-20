using Coms.Application.Services.Common;
using Coms.Application.Services.Contracts;
using Coms.Contracts.Contracts;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    public class ContractsController : ApiController
    {
        private readonly IContractService _contractService;

        public ContractsController(IContractService contractService)
        {
            _contractService = contractService;
        }

        [HttpGet("yours")]
        [SwaggerOperation(Summary = "Get your contracts in Coms")]
        [Authorize(Roles = "Staff, Manager")]
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

        [HttpGet("author")]
        [SwaggerOperation(Summary = "Check contract author in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult IsAuthor([FromQuery] int contractId)
        {
            ErrorOr<AuthorResult> result = _contractService.IsAuthor(
                int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value), contractId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "Get your contracts in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult Delete([FromQuery] int id)
        {
            ErrorOr<ContractResult> result = _contractService.DeleteContract(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("general-report")]
        [SwaggerOperation(Summary = "Get contract general report in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult GetGeneralReport()
        {
            ErrorOr<IList<GeneralReportResult>> result = _contractService.GetGeneralReport(
                int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value)).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("id")]
        [SwaggerOperation(Summary = "Get contract by id in Coms")]
        public IActionResult GetContractById([FromQuery] int id)
        {
            ErrorOr<ContractResult> result = _contractService.GetContract(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Add a contract in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult Add([FromBody] ContractFormRequest request)
        {
            ErrorOr<string> result = request.Name[0];
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        //[HttpPost("add")]
        //[SwaggerOperation(Summary = "Add a contract in Coms")]
        //[Authorize(Roles = "Staff, Manager")]
        //public IActionResult Add([FromBody]ContractFormRequest request)
        //{
        //    ErrorOr<ContractResult> result =
        //        _contractService.AddContract(request.ContractName,request.Code,request.PartnerId, int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value)
        //        ,request.SignerId, request.TemplateId, request.EffectiveDate, request.Services,request.Status).Result;
        //    return result.Match(
        //        result => Ok(result),
        //        errors => Problem(errors)
        //    );
        //}

        [HttpGet("manager")]
        [SwaggerOperation(Summary = "Get contract list for manager in Coms")]
        [Authorize(Roles = "Manager")]
        public IActionResult GetManagerContracts([FromQuery] YourContractsFilterRequest request)
        {
            ErrorOr<PagingResult<ContractResult>> result = _contractService.GetManagerContracts(
                int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value),
                request.ContractName, request.CreatorName, request.Status, request.CurrentPage,
                request.PageSize).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("partner")]
        [SwaggerOperation(Summary = "Get contract list for partner in Coms")]
        public IActionResult GetPartnerContracts([FromQuery] PartnerContractFilterRequest request)
        {
            ErrorOr<PagingResult<ContractResult>> result = _contractService.GetContractForPartner(
                int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value),
                request.ContractName, request.Code, request.IsApproved, request.CurrentPage,
                request.PageSize).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPut("approveOrReject")]
        [SwaggerOperation(Summary = "Approve a contract by manager in Coms")]
        [Authorize(Roles = "Manager")]
        public IActionResult ApproveContract([FromQuery] int id, bool isApproved)
        {
            ErrorOr<ContractResult> result = _contractService
                    .ApproveContract(id, int.Parse(this.User.Claims.First(i => 
                    i.Type == ClaimTypes.NameIdentifier).Value), isApproved).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
