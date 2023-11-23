using Coms.Application.Services.ActionHistories;
using Coms.Application.Services.ContractCosts;
using Coms.Contracts.ActionHistories;
using Coms.Contracts.ContractCost;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Staff")]
    public class ContractCostController : Controller
    {
        private readonly IContractCostService _contractCostService;
        public ContractCostController(IContractCostService contractCostService)
        {
            _contractCostService = contractCostService;
        }

        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add a contract cost in Coms")]
        public IActionResult Add(ContractCostFormRequest request)
        {
            ErrorOr<ContractCostResult> result =
                _contractCostService.AddContractCost( request.ContractId, request.ServiceId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem()
            );
        }
    }
}
