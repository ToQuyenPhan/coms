using Coms.Application.Services.FlowDetails;
using Coms.Application.Services.Flows;
using Coms.Contracts.FlowDetails;
using Coms.Contracts.Flows;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    public class FlowsController : ApiController
    {
        private readonly IFlowService _flowService;
        public FlowsController(IFlowService flowService)
        {
            _flowService = flowService;
        }
        [Authorize(Roles = "Sale Manager")]
        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add a Flow in Coms")]
        public IActionResult Add([FromBody] FlowFormRequest request)
        {
            ErrorOr<FlowResult> result =
                _flowService.AddFlow((Domain.Enum.CommonStatus)request.Status, (int)request.ContractCategoryId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
