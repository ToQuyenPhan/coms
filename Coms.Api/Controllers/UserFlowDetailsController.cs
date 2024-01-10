using Coms.Application.Services.Common;
using Coms.Application.Services.UserFlowDetails;
using Coms.Contracts.Common.Paging;
using Coms.Contracts.UserFlowDetails;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    public class UserFlowDetailsController : ApiController
    {
        private readonly IUserFlowDetailService _userFlowDetailService;

        public UserFlowDetailsController(IUserFlowDetailService userFlowDetailService)
        {
            _userFlowDetailService = userFlowDetailService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get flow details in a contract in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult GetFlowDetails([FromQuery] UserFlowDetailRequest request)
        {
            ErrorOr<PagingResult<UserFlowDetailResult>> result = 
                    _userFlowDetailService.GetContractFlowDetails(request.ContractId, request.CurrentPage, 
                    request.PageSize).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("notifications")]
        [SwaggerOperation(Summary = "Get new contract notifications in Coms")]
        [Authorize(Roles = "Manager")]
        public IActionResult GetNotificatons([FromQuery] PagingRequest request)
        {
            ErrorOr<PagingResult<NotificationResult>> result = 
                    _userFlowDetailService.GetNotifications(int.Parse(this.User.Claims.First(i => i.Type == 
                    ClaimTypes.NameIdentifier).Value), request.CurrentPage, request.PageSize).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
        [Authorize(Roles = "Sale Manager")]
        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add a contract Flow Detail in Coms")]
        public IActionResult AddContractFlowDetail([FromBody] UserFlowDetailFormRequest request)
        {
            ErrorOr<UserFlowDetailResult> result =
                _userFlowDetailService.AddContractFlowDetail(request.Status, request.FlowDetailId, request.ContractId, request.LiquidationRecordId, request.ContractAnnexId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
