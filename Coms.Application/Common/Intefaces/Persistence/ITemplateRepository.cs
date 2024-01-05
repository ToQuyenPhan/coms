using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface ITemplateRepository
    {
        Task AddTemplate(Template template);
        Task<IList<Template>?> GetTemplates(string templateName, int? contractCategoryId,
                int? templateTypeId, int? status, string email);
        Task<Template?> GetTemplate(int id);
        Task UpdateTemplate(Template template);
        Task<IList<Template>?> GetActivatingTemplates();
        Task<Template?> GetTemplateByContractCategoryIdAndTemplateType(int contractCategoryId, int templateType);
        IList<Template>? GetAllTemplates();
    }
}
