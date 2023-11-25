using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class TemplateFileRepository : ITemplateFileRepository
    {
        private readonly IGenericRepository<TemplateFile> _genericRepository;

        public TemplateFileRepository(IGenericRepository<TemplateFile> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task Add(TemplateFile templateFile)
        {
            await _genericRepository.CreateAsync(templateFile);
        }

        public async Task Update(TemplateFile templateFile)
        {
            await _genericRepository.UpdateAsync(templateFile);
        }

        public async Task<TemplateFile?> GetTemplateFileByTemplateId(int templateId)
        {
            return await _genericRepository
                .FirstOrDefaultAsync(tf => tf.TemplateId.Equals(templateId));
        }
    }
}
