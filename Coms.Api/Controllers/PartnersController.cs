using Coms.Application.Services.Partners;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    public class PartnersController : ApiController
    {
        private readonly IPartnerService _partnerService;
        public PartnersController(IPartnerService partnerService)
        {
            _partnerService = partnerService;
        }

        [HttpGet("active")]
        [SwaggerOperation(Summary = "Get all active partners of Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult GetActiveContractCategories()
        {
            ErrorOr<IList<PartnerResult>> result =
                _partnerService.GetActivePartners();
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("current-partner")]
        [SwaggerOperation(Summary = "Get current partner in Coms")]
        public IActionResult GetCurrentPartner()
        {
            ErrorOr<PartnerResult> result =
                _partnerService.GetPartner(int.Parse(this.User.Claims.First(i =>
                i.Type == ClaimTypes.NameIdentifier).Value)).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get a partner by id in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult GetPartner([FromQuery] int id)
        {
            ErrorOr<PartnerResult> result = _partnerService.GetPartner(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
