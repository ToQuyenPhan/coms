using Coms.Application.Services.Common;
using Coms.Application.Services.Partners;
using Coms.Contracts.Partners;
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
                _partnerService.GetPartner(int.Parse(User.Claims.First(i =>
                i.Type == ClaimTypes.NameIdentifier).Value)).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get a partner by id in Coms")]
        [Authorize(Roles = "Staff, Manager,Sale Manager")]
        public IActionResult GetPartner([FromQuery] int id)
        {
            ErrorOr<PartnerResult> result = _partnerService.GetPartner(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        //get all partners
        [HttpGet("all")]
        [SwaggerOperation(Summary = "Get all partners in Coms")]
        [Authorize(Roles = "Sale Manager, Manager")]
        public IActionResult GetPartners([FromQuery] PartnerFilterRequest request)
        {
            try
            {
                ErrorOr<PagingResult<PartnerResult>> result = _partnerService.GetPartners(request.PartnerId, request.Pepresentative, request.CompanyName, request.Status, request.CurrentPage, request.PageSize);
                return result.Match(
                    result => Ok(result),
                    errors => Problem(errors)
                    );
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }






        //delete partner by id
        [HttpDelete("{partnerId}")]
        [SwaggerOperation(Summary = "Delete partner by id")]
        [Authorize(Roles = "Sale Manager")]
        public IActionResult DeletePartner(int partnerId)
        {
            try
            {
                ErrorOr<PartnerResult> result = _partnerService.DeletePartner(partnerId).Result;
                return result.Match(
                    result => Ok(result),
                    errors => Problem(errors)
                );
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        //add partner
        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add a partner in Coms")]
        [Authorize(Roles = "Sale Manager")]
        public IActionResult AddPartner([FromBody] AddPartnerRequest request)
        {
            try
            {
                ErrorOr<PartnerResult> result = _partnerService.AddPartnerAsync(request.Image, request.Representative, request.RepresentativePosition, request.Email, request.Code, request.Phone, request.Address, request.CompanyName, request.TaxCode).Result;
                return result.Match(
                    result => Ok(result),
                    errors => Problem(errors)
                                                                                                                                                                   );
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        //update partner
        [HttpPut("update")]
        [SwaggerOperation(Summary = "Update a partner in Coms")]
        [Authorize(Roles = "Sale Manager")]
        public IActionResult UpdatePartner([FromQuery] int id, [FromBody] AddPartnerRequest request)
        {
            try
            {
                ErrorOr<PartnerResult> result = _partnerService.UpdatePartner(id, request.Image, request.Representative, request.RepresentativePosition, request.Email, request.Code, request.Phone, request.Address, request.CompanyName, request.TaxCode).Result;
                return result.Match(
                    result => Ok(result),
                    errors => Problem(errors)
                    );
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        //update partner status
        [HttpPut("update-status")]
        [SwaggerOperation(Summary = "Update status of a partner in Coms")]
        [Authorize(Roles = "Sale Manager")]
        public IActionResult UpdatePartnerStatus([FromQuery] int id)
        {
            try
            {
                ErrorOr<PartnerResult> result = _partnerService.UpdatePartnerStatus(id).Result;
                return result.Match(
                                       result => Ok(result),
                                                          errors => Problem(errors)
                                                                             );
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }
    }
}
