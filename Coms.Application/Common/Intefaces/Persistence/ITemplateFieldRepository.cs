using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface ITemplateFieldRepository
    {
        Task AddRangeAsync(List<TemplateField> templateFields);
        Task<IList<TemplateField>?> GetTemplateFieldsByTemplateId(int templateId);
    }
}
