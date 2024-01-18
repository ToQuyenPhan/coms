using Coms.Application.Services.Common;
using Coms.Application.Services.ContractAnnexes;
using Coms.Contracts.ContractAnnexes;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Claims;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    public class ContractAnnexesController : ApiController
    {
        private readonly IContractAnnexesService _contractAnnexesService;

        public ContractAnnexesController(IContractAnnexesService contractAnnexesService)
        {
            _contractAnnexesService = contractAnnexesService;
        }

        //get your contract annexes
        [HttpGet("yours")]
        [SwaggerOperation(summary: "Get your contract annexes in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult GetYourContractAnnexes([FromQuery] ContractAnnexesFilterRequest request)
        {
            try
            {
                ErrorOr<PagingResult<ContractAnnexesResult>> result = _contractAnnexesService.GetYourContractAnnexes(
                                                  int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value),
                                                  request.ContractAnnexName, request.Status, request.IsYours, request.CurrentPage,
                                                  request.PageSize).Result;
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
        //get author contract annexes
        [HttpGet("author")]
        [SwaggerOperation(summary: "Check contract annex author in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult IsAuthor([FromQuery] int contractAnnexId)
        {
            try
            {
                ErrorOr<AuthorResult> result = _contractAnnexesService.IsAuthor(int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value), contractAnnexId).Result;
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
        //get manager contract annexes
        [HttpGet("manager")]
        [SwaggerOperation(summary: "Get manager contract annexes in Coms")]
        [Authorize(Roles = "Manager")]
        public IActionResult GetManagerContractAnnexes([FromQuery] ContractAnnexesFilterRequest request)
        {
            try
            {
                ErrorOr<PagingResult<ContractAnnexesResult>> result = _contractAnnexesService.GetManagerContractAnnexes(
                    int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value),
                    request.ContractAnnexName, (int)request.Status, request.CurrentPage, request.PageSize).Result;
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

        //add get contractannexes by contractId
        [HttpGet("contract")]
        [SwaggerOperation(summary: "Get contract annexes by contractId in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult GetContractAnnexesByContractId([FromQuery][Required] int contractId, [FromQuery] ContractAnnexesFilterRequest request)
        {
            try
            {
                ErrorOr<PagingResult<ContractAnnexesResult>> result = _contractAnnexesService.GetContractAnnexesByContractId(
                                                  contractId, request.ContractAnnexName, request.Status, request.CurrentPage,
                                                  request.PageSize).Result;
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

        //add get contractannexes by contractAnnexId
        [HttpGet("id")]
        [SwaggerOperation(summary: "Get contract annexes by contractAnnexId in Coms")]
        public IActionResult GetContractAnnexesById([FromQuery][Required] int id)
        {
            try
            {
                ErrorOr<ContractAnnexesResult> result = _contractAnnexesService.GetContractAnnexesById(id).Result;
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

        //preview contract annex
        [HttpPost("preview")]
        [SwaggerOperation(summary: "Preview contract annex in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult PreviewContractAnnex([FromBody] ContractAnnexPreviewRequest request)
        {
            ErrorOr<MemoryStream> result = _contractAnnexesService.PreviewContractAnnex(request.Name, request.Value,
                                   request.ContractCategoryId, request.TemplateType).Result;
            return result.Match(
                               result => new FileStreamResult(result, "application/pdf"),
                                              errors => Problem(errors)
                                                         );
        }

        [HttpPost("id")]
        [SwaggerOperation(summary: "Upload contract annexes by contractAnnexId in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult UploadContractAnnex([FromQuery] int id)
        {
            ErrorOr<string> result = _contractAnnexesService.UploadContractAnnex(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        //delete contractannexes by contractAnnexId
        [HttpDelete("id")]
        [SwaggerOperation(summary: "Delete contract annexes by contractAnnexId in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult DeleteContractAnnex([FromQuery][Required] int id)
        {
            try
            {
                ErrorOr<ContractAnnexesResult> result = _contractAnnexesService.DeleteContractAnnex(id).Result;
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



        //Get contract annexes list for partner
        [HttpGet("partner")]
        [SwaggerOperation(summary: "Get contract annexes list for partner in Coms")]
        public IActionResult GetPartnerContractAnnexes([FromQuery] PartnerContractAnnexesFilterRequest request)
        {
            try
            {
                ErrorOr<PagingResult<ContractAnnexesResult>> result = _contractAnnexesService.GetContractAnnexForPartnerCode(
                                       int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value),
                                                          request.ContractAnnexName, request.DocumentStatus, request.IsApproved, request.CurrentPage, request.PageSize).Result;
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


        //GetManagerContractsForSign
        [HttpGet("manager/sign")]
        [SwaggerOperation(Summary = "Get contract annexes list for manager to sign in Coms")]
        [Authorize(Roles = "Manager")]
        public IActionResult GetManagerContractAnnexesForSign([FromQuery] ContractAnnexesFilterRequest request)
        {
            try
            {
                ErrorOr<PagingResult<ContractAnnexesResult>> result = _contractAnnexesService.GetManagerContractAnnexesForSign(
                    int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value),
                    request.ContractAnnexName, (int)request.Status, request.CurrentPage, request.PageSize).Result;
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


        //approve or reject contract annex
        [HttpPut("approveOrReject")]
        [SwaggerOperation(summary: "Approve or reject a contract annex by manager in Coms")]
        [Authorize(Roles = "Manager")]
        public IActionResult ApproveContractAnnex([FromQuery] int id, bool isApproved)
        {
            try
            {
                ErrorOr<ContractAnnexesResult> result = _contractAnnexesService
                        .ApproveContractAnnex(id, int.Parse(this.User.Claims.First(i => 
                        i.Type == ClaimTypes.NameIdentifier).Value), isApproved).Result;
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


        //add contract annex
        [HttpPost]
        [SwaggerOperation(summary: "Add a contract annex in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult AddContractAnnex([FromBody] ContractAnnexFormRequest request)
        {
            try
            {
                ErrorOr<int> result = _contractAnnexesService.AddContractAnnex(request.Name, request.Value, request.ContractId,(int)request.ContractCategoryId,
                                               request.ServiceId, request.EffectiveDate, request.Status,
                                                                          int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value), request.ApproveDate,
                                                                                                     request.SignDate, request.PartnerId, (int)request.TemplateType).Result;
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

        [HttpPut("reject")]
        [SwaggerOperation(Summary = "Reject a contract annex by a partner in Coms")]
        public IActionResult RejectContract([FromQuery] int id, bool isApproved)
        {
            ErrorOr<ContractAnnexesResult> result = _contractAnnexesService.RejectContractAnnex(id, isApproved).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

    }
}
