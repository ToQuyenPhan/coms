using Coms.Application.Services.FlowDetails;
using Coms.Application.Services.Partners;
using Coms.Application.Services.Services;
using Coms.Contracts.FlowDetails;
using Coms.Contracts.Partners;
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
        [HttpPut("update")]
        [SwaggerOperation(Summary = "Update a flow detail in Coms")]
        [Authorize(Roles = "Sale Manager")]
        public IActionResult Update([FromQuery] int id, [FromBody] FlowDetailFormRequest request)
        {
            try
            {
                ErrorOr<FlowDetailResult> result = _flowDetailService.UpdateFlowDetail(id, request.FlowRole, request.Order, request.FlowId, request.UserId).Result;
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
        [HttpGet("getById")]
        [SwaggerOperation(Summary = "Get flow detail by id in Coms")]
        [Authorize(Roles = "Sale Manager")]
        public IActionResult GetFlowDetailById([FromQuery] int id)
        {
            ErrorOr<FlowDetailResult> result = _flowDetailService.GetFlowDetail(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
        [HttpGet("getByFlowId")]
        [SwaggerOperation(Summary = "Get flow detail by flow id in Coms")]
        [Authorize(Roles = "Sale Manager")]
        public IActionResult GetFlowDetailByFlowId([FromQuery] int flowId)
        {
            ErrorOr<IList<FlowDetailResult>> result = _flowDetailService.GetFlowDetailByFlowId(flowId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
