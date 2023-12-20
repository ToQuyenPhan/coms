using Coms.Application.Services.Common;
using Coms.Application.Services.UserFlowDetails;
using Coms.Contracts.UserFlowDetails;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Staff, Manager")]
    public class UserFlowDetailsController : ApiController
    {
        private readonly IUserFlowDetailService _userFlowDetailService;

        public UserFlowDetailsController(IUserFlowDetailService userFlowDetailService)
        {
            _userFlowDetailService = userFlowDetailService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get flow details in a contract in Coms")]
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
    }
}
