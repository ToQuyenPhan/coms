using Coms.Application.Services.TemplateTypes;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Sale Manager")]
    public class TemplateTypesController : ApiController
    {
        private readonly ITemplateTypeService _templateTypeService;

        public TemplateTypesController(ITemplateTypeService templateTypeService)
        {
            _templateTypeService = templateTypeService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            ErrorOr<IList<TemplateTypeResult>> result =
                _templateTypeService.GetAllTemplateTypes();
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
