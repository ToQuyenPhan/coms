using Coms.Application.Services.FlowDetails;
using Coms.Application.Services.Services;
using Coms.Contracts.FlowDetails;
using Coms.Contracts.Services;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    public class FlowDetailsController : ApiController
    {
        private readonly IFlowDetailService _flowDetailService;
        public FlowDetailsController(IFlowDetailService flowDetailService)
        {
            _flowDetailService = flowDetailService;
        }
        [Authorize(Roles = "Sale Manager")]
        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add a Flow Detail in Coms")]
        public IActionResult Add([FromBody] FlowDetailFormRequest request)
        {
            ErrorOr<FlowDetailResult> result =
                _flowDetailService.AddFlowDetail(request.FlowRole, request.Order, request.FlowId,request.UserId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
