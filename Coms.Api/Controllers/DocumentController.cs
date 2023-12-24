using Coms.Application.Services.ContractFiles;
using Coms.Application.Services.Contracts;
using Coms.Application.Services.Documents;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Coms.Api.Controllers
{
    [Route("api/document")]
    [Authorize(Roles = "Manager")]
    public class DocumentController : ApiController
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpGet("content")]
        [SwaggerOperation(Summary = "Get content of a Document")]
        public IActionResult GetUserById([FromQuery] Guid id, Guid versionId)
        {
            ErrorOr<ResponseModel> result = _documentService.DownloadDocument(id, versionId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
