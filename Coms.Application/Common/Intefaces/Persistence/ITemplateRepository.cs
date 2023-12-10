using Coms.Application.Services.Common;
using Coms.Application.Services.Templates;
using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface ITemplateRepository
    {
        Task AddTemplate(Template template);
        Task<PagingResult<Template>> GetTemplates(string templateName, int? contractCategoryId,
                int? templateTypeId, int? status, string email, int currentPage, int pageSize);
        Task<Template?> GetTemplate(int id);
        Task UpdateTemplate(Template template);
        Task<IList<Template>?> GetActivatingTemplates();
    }
}
