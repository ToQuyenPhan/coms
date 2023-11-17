using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class TemplateRepository : ITemplateRepository
    {
        private readonly IGenericRepository<Template> _genericRepository;

        public TemplateRepository(IGenericRepository<Template> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task AddTemplate(Template template)
        {
            await _genericRepository.CreateAsync(template);
        }
    }
}
