using Coms.Application.Services.Common;
using Coms.Application.Services.Contracts;
using Coms.Contracts.Attachments;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Staff, Manager")]
    public class AttachmentsController : ApiController
    {
        private readonly IAttachmentService _attachmentService;

        public AttachmentsController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        [HttpGet("all")]
        [SwaggerOperation(Summary = "Get all attachments of a contract in Coms")]
        public IActionResult GetAttachmentsByContractId([FromQuery] AttachmentRequest request)
        {
            ErrorOr<PagingResult<AttachmentResult>> result = 
                    _attachmentService.GetAttachmentsByContractId(request.ContractId, request.CurrentPage, 
                    request.PageSize).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
                 );
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "Delete an attachment of a contract in Coms")]
        public IActionResult DeleteAttachment([FromQuery] int id)
        {
            ErrorOr<AttachmentResult> result = _attachmentService.DeleteAttachment(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
                 );
        }
    }
}
