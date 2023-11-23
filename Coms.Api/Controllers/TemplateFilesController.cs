using Coms.Application.Services.TemplateFiles;
using Coms.Application.Services.Templates;
using Coms.Contracts.Templates;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    public class TemplateFilesController : ApiController
    {
        private readonly ITemplateFileService _templateFileService;

        public TemplateFilesController(ITemplateFileService templateFileService)
        {
            _templateFileService = templateFileService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Add(IFormFile uploadedFile)
        {
            var ms = new MemoryStream();
            uploadedFile.CopyTo(ms);
            var fileContent = ms.ToArray();
            ErrorOr<TemplateFileResult> result = _templateFileService.Add("example", "docx", "document", fileContent, 
               (int)uploadedFile.Length, 7).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
