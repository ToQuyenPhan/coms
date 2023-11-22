using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface ITemplateFileRepository
    {
        Task Add(TemplateFile templateFile);
        Task<TemplateFile?> GetTemplateFileByTemplateId(int templateId);
    }
}
