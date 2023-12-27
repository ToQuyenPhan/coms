using Coms.Api.Common.Request;
using Coms.Application.Services.Accesses;
using Coms.Application.Services.ContractFiles;
using Coms.Application.Services.Contracts;
using Coms.Application.Services.Signs;
using Coms.Contracts.ActionHistories;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Coms.Api.Controllers
{
    [Route("api/sign")]
    [Authorize(Roles = "Manager")]
    public class SignController : ApiController
    {
        private readonly ISignService _signService;
        public SignController(ISignService signService)
        {
            _signService = signService;
        }

        [HttpPost("document/upload-version")]
        [SwaggerOperation(Summary = "Upload and verify signature on version signed file ")]
        public IActionResult Add([FromQuery] Guid fileId, [FromForm] FormUploadRequest file)
        {
            var ms = new MemoryStream();
            file.File.CopyTo(ms);
            var fileContent = ms.ToArray();
            ErrorOr<ResponseModel> result = _signService.UploadVersion(fileId, fileContent)
                    .Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
