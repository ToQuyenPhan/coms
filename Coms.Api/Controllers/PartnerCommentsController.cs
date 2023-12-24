using Coms.Application.Services.Common;
using Coms.Application.Services.PartnerComments;
using Coms.Contracts.Common.Paging;
using ErrorOr;
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
        public IActionResult GetComments([FromQuery] int contractId, [FromQuery]PagingRequest request)
        {
            ErrorOr<PagingResult<PartnerCommentResult>> result =
                _partnerCommentService.GetPartnerComments(contractId, request.CurrentPage, request.PageSize).Result;
            return result.Match(result => Ok(result), errors => Problem(errors));
        }
    }
}
