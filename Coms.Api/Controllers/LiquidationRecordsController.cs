using Coms.Application.Services.Common;
using Coms.Application.Services.LiquidationRecords;
using Coms.Contracts.LiquidationRecords;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    public class LiquidationRecordsController : ApiController
    {
        private readonly ILiquidationRecordsService _liquidationRecordsService;

        public LiquidationRecordsController(ILiquidationRecordsService liquidationRecordsService)
        {
            _liquidationRecordsService = liquidationRecordsService;
        }

        [HttpGet("all")]
        [SwaggerOperation(summary: "Get all Liquidation Record in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult GetAllLiquidationRecords([FromQuery] LiquidationRecordFilterRequest request)
        {
            try
            {
                ErrorOr<PagingResult<LiquidationRecordsResult>> result = _liquidationRecordsService.GetLiquidationRecords(
                               request.LiquidationRecordName, request.Status, request.CurrentPage,
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

        [HttpGet("contract")]
        [SwaggerOperation(summary: "Get Liquidation Record by contractId in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult GetLiquidationRecordsByContractId([FromQuery][Required] int contractId, [FromQuery] LiquidationRecordFilterRequest request)
        {
            try
            {
                ErrorOr<PagingResult<LiquidationRecordsResult>> result = _liquidationRecordsService.GetLiquidationRecordsByContractId(
                                                  contractId, request.LiquidationRecordName, request.Status, request.CurrentPage,
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


        [HttpGet("id")]
        [SwaggerOperation(summary: "Get Liquidation Record by LiquidationRecordId in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult GetLiquidationRecordsById([FromQuery][Required] int id)
        {
            try
            {
                ErrorOr<LiquidationRecordsResult> result = _liquidationRecordsService.GetLiquidationRecordsById(id).Result;
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

        [HttpDelete("id")]
        [SwaggerOperation(summary: "Delete Liquidation Record by LiquidationRecordId in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult DeleteLiquidationRecord([FromQuery][Required] int id)
        {
            try
            {
                ErrorOr<LiquidationRecordsResult> result = _liquidationRecordsService.DeleteLiquidationRecords(id).Result;
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
        [HttpDelete("id")]
        [SwaggerOperation(summary: "Delete Liquidation Record by LiquidationRecordId in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult DeleteLiquidationRecorddd([FromQuery][Required] int id)
        {
            try
            {
                ErrorOr<LiquidationRecordsResult> result = _liquidationRecordsService.DeleteLiquidationRecords(id).Result;
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
