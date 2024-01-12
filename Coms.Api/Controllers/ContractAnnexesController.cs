using Coms.Application.Services.Common;
using Coms.Application.Services.ContractAnnexes;
using Coms.Contracts.ContractAnnexes;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
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
        [Authorize(Roles = "Staff, Manager")]
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

        //manage sign contract annex
        //[HttpPut("manager/sign")]
        //[SwaggerOperation(summary: "Manager sign a contract annex in Coms")]
        //[Authorize(Roles = "Manager")]
        //public IActionResult ManagerSignContractAnnex([FromQuery] int id)
        //{
        //    try
        //    {
        //        ErrorOr<ContractAnnexesResult> result = _contractAnnexesService
        //                .ManagerSignContractAnnex(id, int.Parse(this.User.Claims.First(i =>
        //                                                              i.Type == ClaimTypes.NameIdentifier).Value)).Result;
        //        return result.Match(
        //                                                  result => Ok(result),
        //                                                                                                           errors => Problem(errors)
        //                                                                                                                                                                                   );
        //    }
        //    catch (Exception e)
        //    {
        //        return Problem(e.Message);
        //    }

        //}
       

    }
}
