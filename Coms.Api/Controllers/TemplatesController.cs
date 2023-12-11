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
    public class TemplatesController : ApiController
    {
        private readonly ITemplateService _templateService;

        public TemplatesController(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all templates (with filter) in Coms")]
        [Authorize(Roles = "Sale Manager, Staff, Manager")]
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
        [Authorize(Roles = "Sale Manager")]
        public IActionResult Add(TemplateFormRequest request)
        {
            ErrorOr<TemplateResult> result =
                _templateService.AddTemplate(request.TemplateName, request.Description, 
                    request.ContractCategoryId, request.TemplateTypeId, request.Status, 
                    int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier)
                    .Value)).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "Delete a template in Coms")]
        [Authorize(Roles = "Sale Manager")]
        public IActionResult Delete(int id)
        {
            ErrorOr<TemplateResult> result = _templateService.DeleteTemplate(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("get-template")]
        [SwaggerOperation(Summary = "Get a template in Coms")]
        [Authorize(Roles = "Sale Manager, Staff")]
        public async Task<IActionResult> GetTemplate([FromQuery]int id)
        {
            ErrorOr<TemplateSfdtResult> result = await _templateService.GetTemplate(id);
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Edit a template in Coms")]
        [Authorize(Roles = "Sale Manager")]
        public IActionResult Edit(TemplateFormRequest request, [FromQuery] int templateId)
        {
            ErrorOr<TemplateResult> result =
                _templateService.UpdateTemplate(request.TemplateName, request.Description,
                    request.ContractCategoryId, request.TemplateTypeId, request.Status,
                    templateId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("get-template-info")]
        [SwaggerOperation(Summary = "Get a template information in Coms")]
        [Authorize(Roles = "Sale Manager")]
        public async Task<IActionResult> GetTemplateInformation([FromQuery] int id)
        {
            ErrorOr<TemplateResult> result = _templateService.GetTemplateInformation(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPut("activate")]
        [SwaggerOperation(Summary = "Activate a template in Coms")]
        [Authorize(Roles = "Sale Manager")]
        public async Task<IActionResult> ActivateTemplate([FromQuery] int id)
        {
            ErrorOr<TemplateResult> result = await _templateService.ActivateTemplate(id);
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPut("deactivate")]
        [SwaggerOperation(Summary = "Deactivate a template in Coms")]
        [Authorize(Roles = "Sale Manager")]
        public async Task<IActionResult> DeactivateTemplate([FromQuery] int id)
        {
            ErrorOr<TemplateResult> result = await _templateService.DeactivateTemplate(id);
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
