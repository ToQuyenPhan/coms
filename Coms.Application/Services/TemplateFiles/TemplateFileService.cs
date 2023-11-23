using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Templates;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;

namespace Coms.Application.Services.TemplateFiles
{
    public class TemplateFileService : ITemplateFileService
    {
        private readonly ITemplateFileRepository _templateFileRepository;

        public TemplateFileService(ITemplateFileRepository templateFileRepository)
        {
            _templateFileRepository = templateFileRepository;
        }

        public async Task<ErrorOr<TemplateFileResult>> Add(string name, string extension, string contenType, byte[] document,
                int size, int templateId)
        {
            try
            {
                var templateFile = new TemplateFile()
                {
                    FileName = name,
                    Extension = extension,
                    ContentType = contenType,
                    FileData = document,
                    FileSize = size,
                    UploadedDate = DateTime.UtcNow,
                    TemplateId = templateId
                };
                await _templateFileRepository.Add(templateFile);
                return new TemplateFileResult() { Result = "OK" };
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }
    }
}
