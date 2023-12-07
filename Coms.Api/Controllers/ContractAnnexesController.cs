using Coms.Application.Services.Common;
using Coms.Application.Services.ContractAnnexes;
using Coms.Contracts.ContractAnnexes;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

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

        //[HttpGet("yours")]
        //[SwaggerOperation(Summary = "Get your contracts in Coms")]
        //[Authorize(Roles = "Staff, Manager")]
        //public IActionResult GetYourContracts([FromQuery] YourContractsFilterRequest request)
        //{
        //    ErrorOr<PagingResult<ContractResult>> result = _contractService.GetYourContracts(
        //        int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value),
        //        request.ContractName, request.CreatorName, request.Status, request.CurrentPage,
        //        request.PageSize).Result;
        //    return result.Match(
        //        result => Ok(result),
        //        errors => Problem(errors)
        //    );
        //}

        //add get all contractannex
        [HttpGet("all")]
        [SwaggerOperation(summary: "Get all contract annexes in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult GetAllContractAnnexes([FromQuery] ContractAnnexesFilterRequest request)
        {
            try
            {
                ErrorOr<PagingResult<ContractAnnexesResult>> result = _contractAnnexesService.GetContractAnnexes(
                               request.ContractAnnexName, request.Status, request.CurrentPage,
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
        public IActionResult Delete([FromQuery][Required] int id)
        {
            try
            {
                ErrorOr<ContractAnnexesResult> result = _contractAnnexesService.Delete(id).Result;
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
