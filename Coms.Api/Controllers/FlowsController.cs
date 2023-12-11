using Coms.Application.Services.Accesses;
using Coms.Application.Services.Flows;
using Coms.Contracts.Flows;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Sale Manager")]
    public class FlowsController : ApiController
    {
        private readonly IFlowService _flowService;
        public FlowsController(IFlowService flowService)
        {
            _flowService = flowService;
        }

        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add a Flow in Coms")]
        public IActionResult Add([FromBody]FlowFormRequest request)
        {
            ErrorOr<FlowResult> result =
                _flowService.AddFlow(request.ContractCategoryId, request.Status).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
