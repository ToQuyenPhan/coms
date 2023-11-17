using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface ITemplateRepository
    {
        Task AddTemplate(Template template);
    }
}
