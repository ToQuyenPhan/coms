using Coms.Application.Services.Common;
using Coms.Application.Services.Contracts;
using Coms.Contracts.Contracts;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Staff")]
    public class AttachmentsController : ApiController
    {
        private readonly IAttachmentService _attachmentService;

        public AttachmentsController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        //add get all attachments by "contract id"
        [HttpGet("{contractId}")]
        [SwaggerOperation(Summary = "Get all attachments of a contract in Coms")]
        public IActionResult GetAttachmentsByContractId(int contractId)
        {
            ErrorOr<IList<AttachmentResult>> result = _attachmentService.GetAttachmentsByContractId(contractId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
                 );
        }
        

    }
}
