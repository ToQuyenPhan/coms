using Coms.Api.Common.Request;
using Coms.Application.Services.Signs;
using Coms.Contracts.Signs;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Coms.Api.Controllers
{
    [Route("api/sign")]
    //[Authorize(Roles = "Manager, Partner")]
    [AllowAnonymous]
    public class SignController : ApiController
    {
        private readonly ISignService _signService;
        public SignController(ISignService signService)
        {
            _signService = signService;
        }

        [HttpPost("document/upload-version")]
        [SwaggerOperation(Summary = "Upload and verify signature on version signed file ")]
        public IActionResult UploadVersion([FromQuery] Guid id, [FromForm]UploadRequest request)
        {
            var ms = new MemoryStream();
            request.File.CopyTo(ms);
            var fileContent = ms.ToArray();
            ErrorOr<ResponseModel> result = _signService.UploadVersion(id, fileContent)
                    .Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
