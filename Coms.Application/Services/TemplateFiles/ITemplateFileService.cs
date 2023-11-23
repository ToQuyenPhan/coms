using ErrorOr;

namespace Coms.Application.Services.TemplateFiles
{
    public interface ITemplateFileService
    {
        Task<ErrorOr<TemplateFileResult>> Add(string name, string extension, string contenType, byte[] document,
                int size, int templateId);
        Task<ErrorOr<TemplateFileResult>> ExportPDf(string content, int id);
    }
}
