using Coms.Application.Services.PartnerComments;
using Coms.Contracts.PartnerComments;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    public class PartnerCommentsController : ApiController
    {
        private readonly IPartnerCommentService _partnerCommentService;

        public PartnerCommentsController(IPartnerCommentService partnerCommentService)
        {
            _partnerCommentService = partnerCommentService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all partner comments of a contract in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult GetComments([FromQuery] int contractId)
        {
            ErrorOr<PartnerCommentResult> result = _partnerCommentService.GetPartnerComment(contractId).Result;
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

        //get all partner comments of a contract annex
        [HttpGet("annex")]
        [SwaggerOperation(Summary = "Get all partner comments of a contract annex in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult GetAnnexComments([FromQuery] int contractAnnexId)
        {
            ErrorOr<PartnerCommentResult> result = _partnerCommentService.GetPartnerCommentByContractAnnexId(contractAnnexId).Result;
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Partner comment for a contract in Coms")]
        public IActionResult AddComment([FromBody] PartnerCommentFormRequest request)
        {
            ErrorOr<PartnerCommentResult> result = _partnerCommentService.AddPartnerComment(request.ContractId, 
                    request.Content).Result;
            return result.Match(result => Ok(result), errors => Problem(errors));
        }
    }
}
