using Coms.Application.Services.ActionHistories;
using Coms.Application.Services.Common;
using Coms.Contracts.ActionHistories;
using Coms.Contracts.Common.Paging;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [AllowAnonymous]
    public class ActionHistoriesController : ApiController
    {
        private readonly IActionHistoryService _actionHistoryService;

        public ActionHistoriesController(IActionHistoryService actionHistoryService)
        {
            _actionHistoryService = actionHistoryService;
        }

        [HttpGet("contract")]
        [SwaggerOperation(Summary = "Get action history by contract id in Coms")]
        public IActionResult GetActionHistoryByContractId([FromQuery] int contractId, [FromQuery] PagingRequest request)
        {
            ErrorOr<PagingResult<ActionHistoryResult>> result = _actionHistoryService.GetActionHistoryByContractId(contractId, request.CurrentPage,
                    request.PageSize).Result;
            return result.Match(
                               result => Ok(result),
                                              errors => Problem(errors)
                                                         );
        }

        [HttpGet("recent")]
        [SwaggerOperation(Summary = "Get recent activities of your contracts in Coms")]
        public IActionResult GetRecentActivities([FromQuery] PagingRequest request)
        {
            ErrorOr<PagingResult<ActionHistoryResult>> result =
                _actionHistoryService.GetRecentActivities(int.Parse(this.User.Claims.First(i => i.Type ==
                ClaimTypes.NameIdentifier).Value), request.CurrentPage, request.PageSize).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("export")]
        [SwaggerOperation(Summary = "Export action activities of your contracts in Coms")]
        public IActionResult ExportExcel()
        {
            var fileDownloadName = "action-reports.xlsx";
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            ErrorOr<MemoryStream> result =
                _actionHistoryService.ExportActionHistories(int.Parse(this.User.Claims.First(i => i.Type ==
                ClaimTypes.NameIdentifier).Value)).Result;
            var fsr = new FileStreamResult(result.Value, contentType);
            fsr.FileDownloadName = fileDownloadName;
            return result.Match(
                result => fsr,
                errors => Problem(errors)
            );
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add a action history in Coms")]
        public IActionResult Add(ActionHistoryFormRequest request)
        {
            ErrorOr<ActionHistoryResult> result =
                _actionHistoryService.AddActionHistory(int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value), request.ContractId, request.ActionType).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
