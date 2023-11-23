using Coms.Application.Services.Accesses;
using Coms.Application.Services.ActionHistories;
using Coms.Contracts.ActionHistories;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Staff")]
    public class ActionHistoryController : Controller
    {
        private readonly IActionHistoryService _actionHistoryService;
        public ActionHistoryController(IActionHistoryService actionHistoryService)
        {
            _actionHistoryService = actionHistoryService;
        }

        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add a action history in Coms")]
        public IActionResult Add(ActionHistoryFormRequest request)
        {
            ErrorOr<ActionHistoryResult> result =
                _actionHistoryService.AddActionHistory(int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value), request.ContractId, request.ActionType).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem()
            );
        }
    }
}
