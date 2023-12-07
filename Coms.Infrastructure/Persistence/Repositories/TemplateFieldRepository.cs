using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class TemplateFieldRepository : ITemplateFieldRepository
    {
        private readonly IGenericRepository<TemplateField> _genericRepository;

        public TemplateFieldRepository(IGenericRepository<TemplateField> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task AddRangeAsync(List<TemplateField> templateFields)
        {
            await _genericRepository.CreateRangeAsync(templateFields);
        }
    }
}
