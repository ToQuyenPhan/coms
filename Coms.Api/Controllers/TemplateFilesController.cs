using Coms.Api.Common.Request;
using Coms.Application.Services.TemplateFiles;
using Coms.Contracts.TemplateFiles;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Sale Manager")]
    public class TemplateFilesController : ApiController
    {
        private readonly ITemplateFileService _templateFileService;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment hostEnvironment;

        public TemplateFilesController(ITemplateFileService templateFileService, 
                Microsoft.AspNetCore.Hosting.IHostingEnvironment hostEnvironment)
        {
            _templateFileService = templateFileService;
            this.hostEnvironment = hostEnvironment;
        }

        [HttpPost]
        public IActionResult Add([FromQuery] int templateId, string? templateName, [FromForm]FormUploadRequest file)
        {
            string path = this.hostEnvironment.WebRootPath + "\\Files\\";
            var ms = new MemoryStream();
            file.File.CopyTo(ms);
            var fileContent = ms.ToArray();
            ErrorOr<TemplateFileResult> result = _templateFileService.Add(templateName, "docx", 
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document", 
                fileContent, (int)file.File.Length, templateId, path).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPost("pdf")]
        public async Task<IActionResult> Pdf([FromQuery] int id, [FromBody] PdfDataRequest request)
        {
            string path = this.hostEnvironment.WebRootPath + "\\Files\\";
            ErrorOr<TemplateFileResult> result = await _templateFileService.ExportPDf(request.Content, id, path);
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPost("update-template")]
        public IActionResult Update([FromQuery] int templateId, string templateName, [FromForm] FormUploadRequest file)
        {
            var ms = new MemoryStream();
            file.File.CopyTo(ms);
            var fileContent = ms.ToArray();
            ErrorOr<TemplateFileResult> result = _templateFileService.Update(templateId, templateName,
                    fileContent)
                    .Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
