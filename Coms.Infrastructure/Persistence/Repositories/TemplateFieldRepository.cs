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

        public async Task<IList<TemplateField>?> GetTemplateFieldsByTemplateId(int templateId)
        {
            var list = await _genericRepository.WhereAsync(tf => tf.TemplateId.Equals(templateId), null);
            return (list.Count() > 0) ? list : null;
        }

        public async Task UpdateRangeAsync(List<TemplateField> templateFields)
        {
            await _genericRepository.UpdateRangeAsync(templateFields);
        }

        public async Task DeleteRangeAsync(List<TemplateField> templateFields)
        {
            await _genericRepository.DeleteRangeAsync(templateFields);
        }
    }
}
