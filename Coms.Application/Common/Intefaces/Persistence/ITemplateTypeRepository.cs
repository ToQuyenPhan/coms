using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface ITemplateTypeRepository
    {
        Task<IList<TemplateType>?> GetAllTemplateTypes();
        Task<TemplateType?> GetTemplateTypeById(int id);
    }
}
