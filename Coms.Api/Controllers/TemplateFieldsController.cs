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
        public IActionResult GetTemplateFields([FromQuery] int contractCategoryId, int partnerId)
        {
            ErrorOr<IList<TemplateFieldResult>> result = _templateFieldService.GetTemplateFields(contractCategoryId, 
                    partnerId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
