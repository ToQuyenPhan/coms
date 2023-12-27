using Coms.Api.Common.Request;
using Coms.Application.Services.ContractFiles;
using Coms.Application.Services.TemplateFiles;
using Coms.Application.Services.Users;
using Coms.Contracts.TemplateFiles;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    //[Authorize(Roles = "Staff")]
    [AllowAnonymous]
    public class ContractFilesController : ApiController
    {
        private readonly IContractFileService _contractFileService;

        public ContractFilesController(IContractFileService contractFileService)
        {
            _contractFileService = contractFileService;
        }

        [HttpPost]
        public IActionResult Add([FromQuery] int contractId, string? contractName, [FromForm] FormUploadRequest file)
        {
            var ms = new MemoryStream();
            file.File.CopyTo(ms);
            var fileContent = ms.ToArray();
            ErrorOr<ContractFileResult> result = _contractFileService.Add(contractName, "docx",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                fileContent, (int)file.File.Length, contractId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem()
            );
        }

        [HttpPost("pdf")]
        public IActionResult Pdf([FromQuery] int id, [FromBody] PdfDataRequest request)
        {
            ErrorOr<ContractFileResult> result = _contractFileService.ExportPDf(request.Content, id)
                .Result;
            return result.Match(
                result => Ok(result),
                errors => Problem()
            );
        }

        [HttpGet("contractId")]
        [SwaggerOperation(Summary = "Get contractFile by contractId in Coms")]
        public IActionResult GetContracFileByContractId([FromQuery] int contractId)
        {
            ErrorOr<ContractFileObjectResult> result = _contractFileService.GetContracFile(contractId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
