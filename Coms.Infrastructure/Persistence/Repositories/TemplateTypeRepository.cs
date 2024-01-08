using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class TemplateTypeRepository : ITemplateTypeRepository
    {
        private readonly IGenericRepository<TemplateType> _genericRepository;

        public TemplateTypeRepository(IGenericRepository<TemplateType> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<IList<TemplateType>?> GetAllTemplateTypes()
        {
            var list = _genericRepository.GetAll();
            return (list.Count > 0) ? list : null;
        }

        public async Task<TemplateType?> GetTemplateTypeById(int id)
        {
            var item = await _genericRepository.FirstOrDefaultAsync(cc => cc.Id == id);
            return item;
        }
    }
}
