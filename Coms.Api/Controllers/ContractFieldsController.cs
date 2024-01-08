using Coms.Application.Services.ContractFields;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Staff, Manager")]
    public class ContractFieldsController : ApiController
    {
        private readonly IContractFieldService _contractFieldService;

        public ContractFieldsController(IContractFieldService contractFieldService)
        {
            _contractFieldService = contractFieldService;
        }

        [HttpGet]
        public IActionResult GetContractFields([FromQuery] int contractId, int partnerId, int serviceId)
        {
            ErrorOr<IList<ContractFieldResult>> result = 
                    _contractFieldService.GetContractFields(contractId, partnerId, serviceId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
