using Coms.Application.Services.Attachments;
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
        

        [HttpPost]
        [SwaggerOperation(Summary = "Add an attachment of a contract in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult AddAttachment([FromBody] AddAttachmentRequest request)
        {
            ErrorOr<AttachmentResult> result = _attachmentService.AddAttachment(request.FileName, request.FileLink, request.UploadDate, request.Description, request.ContractId).Result;
            return result.Match(
                               result => Ok(result),
                                              errors => Problem(errors)
                                                              );
        }
        

        [HttpDelete]
        [SwaggerOperation(Summary = "Delete an attachment of a contract in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult DeleteAttachment([FromQuery] int id)
        {
            ErrorOr<AttachmentResult> result = _attachmentService.DeleteAttachment(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
                 );
        }
        
        #region ContractAnnex
        //get attachment by contract annex id
        [HttpGet("annex")]
        [SwaggerOperation(Summary = "Get all attachments of a contract annex in Coms")]
        public IActionResult GetAttachmentsByContractAnnexId([FromQuery] AttachmentRequest request)
        {
            ErrorOr<PagingResult<ContractAnnexAttachmentResult>> result =
                    _attachmentService.GetAttachmentsByContractAnnexId(request.ContractAnnexId, request.CurrentPage,
                                       request.PageSize).Result;
            return result.Match(
                               result => Ok(result),
                                              errors => Problem(errors)
                                                              );
        }
        //add attachment of contract annex
        [HttpPost("annex")]
        [SwaggerOperation(Summary = "Add an attachment of a contract annex in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult AddContractAnnexAttachment([FromBody] AddContractAnnexAttachmentRequest request)
        {
            ErrorOr<ContractAnnexAttachmentResult> result = _attachmentService.AddContractAnnexAttachment(request.FileName, request.FileLink, request.UploadDate, request.Description, request.ContractAnnexId).Result;
            return result.Match(
                                              result => Ok(result),
                                                                                           errors => Problem(errors)
                                                                                                                                                        );
        }
        //delete attachment of contract annex
        [HttpDelete("annex")]
        [SwaggerOperation(Summary = "Delete an attachment of a contract annex in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult DeleteContractAnnexAttachment([FromQuery] int id)
        {
            ErrorOr<ContractAnnexAttachmentResult> result = _attachmentService.DeleteContractAnnexAttachment(id).Result;
            return result.Match(
                               result => Ok(result),
                                              errors => Problem(errors)
                                                              );
        }
        #endregion

    }
}
