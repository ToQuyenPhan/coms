using Coms.Application.Services.TemplateFields;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coms.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Staff, Manager")]
    public class TemplateFieldsController : ApiController
    {
        private readonly ITemplateFieldService _templateFieldService;

        public TemplateFieldsController(ITemplateFieldService templateFieldService)
        {
            _templateFieldService = templateFieldService;
        }

        [HttpGet]
        public IActionResult GetTemplateFields([FromQuery] int contractCategoryId, int partnerId, int serviceId, int templateType)
        {
            ErrorOr<IList<TemplateFieldResult>> result = _templateFieldService.GetTemplateFields(contractCategoryId, 
                    partnerId, serviceId, templateType).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
        [HttpGet("Annex")]
        public IActionResult GetTemplateAnnexFields([FromQuery] int contractId, int contractCategoryId, int partnerId, int serviceId, int templateType)
        {
            ErrorOr<IList<TemplateFieldResult>> result = _templateFieldService.GetTemplateAnnexFields( contractId, contractCategoryId,
                    partnerId, serviceId, templateType).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
