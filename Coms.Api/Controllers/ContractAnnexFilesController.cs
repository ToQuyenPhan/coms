using Coms.Application.Services.ContractAnnexFiles;
using Coms.Application.Services.ContractFiles;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    //[Authorize(Roles = "Staff")]
    [AllowAnonymous]
    public class ContractAnnexFilesController : ApiController
    {
        private readonly IContractAnnexFileService _contractAnnexFileService;

        public ContractAnnexFilesController(IContractAnnexFileService contractAnnexFileService)
        {
            _contractAnnexFileService = contractAnnexFileService;
        }

        [HttpGet("contractAnnexId")]
        [SwaggerOperation(Summary = "Get contractAnnexFile by contractAnnexId in Coms")]
        public IActionResult GetContracAnnexFileByContractAnnexId([FromQuery] int contractAnnexId)
        {
            ErrorOr<ContractAnnexFileObjectResult> result = _contractAnnexFileService.GetContractAnnexFileByContractAnnexId(contractAnnexId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
