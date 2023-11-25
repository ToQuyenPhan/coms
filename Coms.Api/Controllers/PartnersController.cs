using Coms.Application.Services.ContractCategories;
using Coms.Application.Services.PartnerReviews;
using Coms.Application.Services.Partners;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Staff")]
    public class PartnersController : ApiController
    {
        private readonly IPartnerService _partnerService;
        public PartnersController(IPartnerService partnerService)
        {
            _partnerService = partnerService;
        }

        [HttpGet("active")]
        [SwaggerOperation(Summary = "Get all active partners of Coms")]
        public IActionResult GetActiveContractCategories()
        {
            ErrorOr<IList<PartnerResult>> result =
                _partnerService.GetActivePartners();
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
