using Coms.Application.Services.Common;
using Coms.Application.Services.Templates;
using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface ITemplateRepository
    {
        Task AddTemplate(Template template);
        Task<PagingResult<Template>> GetTemplates(string templateName, int? contractCategoryId,
                int? templateTypeId, int? status, int currentPage, int pageSize);
        Task DeleteTemplate(Template template);
        Task<Template> GetTemplate(int id);
    }
}
