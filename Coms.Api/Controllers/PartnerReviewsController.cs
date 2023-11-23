using Coms.Application.Services.Accesses;
using Coms.Application.Services.Contracts;
using Coms.Application.Services.PartnerReviews;
using Coms.Contracts.PartnerReviews;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Staff")]
    public class PartnerReviewsController : Controller
    {
        private readonly IPartnerReviewService _partnerReviewService;
        public PartnerReviewsController(IPartnerReviewService partnerReviewService)
        {
            _partnerReviewService = partnerReviewService;
        }

        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add a partner review in Coms")]
        public IActionResult Add(PartnerReviewFormRequest request)
        {
            ErrorOr<PartnerReviewResult> result =
                _partnerReviewService.AddPartnerReview(int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value), request.PartnerId,request.ContractId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem()
            );
        }
    }
}
