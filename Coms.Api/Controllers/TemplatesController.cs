using Coms.Application.Services.Common;
using Coms.Application.Services.Templates;
using Coms.Contracts.Templates;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

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

        [HttpGet]
        [SwaggerOperation(Summary = "Get all templates (with filter) in Coms")]
        public IActionResult Get([FromQuery]TemplateFilterRequest request)
        {
            ErrorOr<PagingResult<TemplateResult>> result =
                _templateService.GetTemplates(request.TemplateName, request.ContractCategoryId, 
                    request.TemplateTypeId, request.Status, request.Creator, request.CurrentPage, request.PageSize).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add a new template in Coms")]
        public IActionResult Add(TemplateFormRequest request)
        {
            ErrorOr<TemplateResult> result =
                _templateService.AddTemplate(request.TemplateName, request.Description, 
                    request.ContractCategoryId, request.TemplateTypeId, request.TemplateLink, 
                    request.Status, int.Parse(this.User.Claims
                    .First(i => i.Type == ClaimTypes.NameIdentifier).Value))
                    .Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "Delete a template in Coms")]
        public IActionResult Delete(int id)
        {
            ErrorOr<TemplateResult> result = _templateService.DeleteTemplate(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Edit a template in Coms")]
        [AllowAnonymous]
        public async Task<IActionResult> Edit([FromQuery]int id)
        {
            ErrorOr<TemplateSfdtResult> result = _templateService.GetTemplate(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
