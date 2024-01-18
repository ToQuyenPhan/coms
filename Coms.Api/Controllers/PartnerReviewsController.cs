using Coms.Application.Services.Common;
using Coms.Application.Services.PartnerReviews;
using Coms.Contracts.Common.Paging;
using Coms.Contracts.PartnerReviews;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    public class PartnerReviewsController : ApiController
    {
        private readonly IPartnerReviewService _partnerReviewService;
        public PartnerReviewsController(IPartnerReviewService partnerReviewService)
        {
            _partnerReviewService = partnerReviewService;
        }

        [HttpGet("notifications")]
        [SwaggerOperation(Summary = "Get partner review notifications in Coms")]
        [Authorize(Roles = "Staff")]
        public IActionResult GetNotifications([FromQuery] PagingRequest request)
        {
            ErrorOr<PagingResult<NotificationResult>> results = 
                    _partnerReviewService.GetNotifications(int.Parse(this.User.Claims.First(i => 
                    i.Type == ClaimTypes.NameIdentifier).Value), request.CurrentPage, request.PageSize).Result;
            return results.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("partner-notifications")]
        [SwaggerOperation(Summary = "Get notifications for partners in Coms")]
        public IActionResult GetPartnerNotifications([FromQuery] PagingRequest request)
        {
            ErrorOr<PagingResult<NotificationResult>> results =
                    _partnerReviewService.GetPartnerNotifications(int.Parse(this.User.Claims.First(i =>
                    i.Type == ClaimTypes.NameIdentifier).Value), request.CurrentPage, request.PageSize).Result;
            return results.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add a partner review in Coms")]
        [Authorize(Roles = "Staff")]
        public IActionResult Add(PartnerReviewFormRequest request)
        {
            ErrorOr<PartnerReviewResult> result =
                _partnerReviewService.AddPartnerReview(int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value), request.PartnerId,request.ContractId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPut("approveOrReject")]
        [SwaggerOperation(Summary = "Approve or reject a contract for partner in Coms")]
        public IActionResult ApproveContract([FromQuery] int contractId, [FromQuery] bool isApproved)
        {
            ErrorOr<PartnerReviewResult> result = _partnerReviewService.ApprovePartnerReview(contractId, isApproved).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPut("annex/approveOrReject")]
        [SwaggerOperation(Summary = "Approve or reject a contract for partner in Coms")]
        public IActionResult ApproveContractAnnex([FromQuery] int contractAnnexId, [FromQuery] bool isApproved)
        {
            ErrorOr<PartnerReviewResult> result = _partnerReviewService.ApproveAnnexPartnerReview(contractAnnexId, isApproved).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
