using Coms.Application.Services.ContractCategories;
using Coms.Application.Services.PartnerReviews;
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
        [Authorize(Roles = "Staff")]
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
        //add partner
        //[HttpPost]
        //[SwaggerOperation(Summary = "Add new partner to Coms")]
        //[Authorize(Roles = "Sale Manager")]
        //public IActionResult AddPartner([FromBody] AddPartnerResult command)
        //{
        //    ErrorOr<PartnerResult> result =
        //        _partnerService.AddPartnerAsync(command);
        //    return result.Match(
        //                       result => Ok(result),
        //                                      errors => Problem(errors)
        //                                                 );
        //}
    }
}
