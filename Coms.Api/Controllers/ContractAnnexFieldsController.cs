using Coms.Application.Services.ContractAnnexFields;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Staff, Manager")]
    public class ContractAnnexFieldsController : ApiController
    {
        private readonly IContractAnnexFieldService _contractAnnexFieldService;

        public ContractAnnexFieldsController(IContractAnnexFieldService contractAnnexFieldService)
        {
            _contractAnnexFieldService = contractAnnexFieldService;
        }

        [HttpGet]
        public IActionResult GetContractAnnexFields([FromQuery] int contractAnnexId, int contractId, int partnerId, int serviceId)
        {
            ErrorOr<IList<ContractAnnexFieldResult>> result =
                    _contractAnnexFieldService.GetContractAnnexFields(contractAnnexId, contractId, partnerId, serviceId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
