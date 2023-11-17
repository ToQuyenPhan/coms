using Coms.Application.Services.Templates;
using Coms.Contracts.Templates;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Sale Manager")]
    public class TemplatesController : ApiController
    {
        private readonly ITemplateService _templateService;

        public TemplatesController(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        [HttpPost("add")]
        public IActionResult Add(TemplateFormRequest request)
        {
            ErrorOr<TemplateResult> result =
                _templateService.AddTemplate(request.TemplateName, request.Description, 
                    request.ContractCategoryId, request.TemplateTypeId, request.TemplateLink, 
                    request.Status).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
