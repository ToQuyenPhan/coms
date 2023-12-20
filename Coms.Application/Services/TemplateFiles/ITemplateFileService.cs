using ErrorOr;

namespace Coms.Application.Services.TemplateFiles
{
    public interface ITemplateFileService
    {
        Task<ErrorOr<TemplateFileResult>> Add(string name, string extension, string contenType, byte[] document,
                int size, int templateId, string serverPath);
        Task<ErrorOr<TemplateFileResult>> ExportPDf(string content, int id, string serverPath);
        Task<ErrorOr<TemplateFileResult>> Update(int templateId, string templateName, byte[] document);
    }
}
