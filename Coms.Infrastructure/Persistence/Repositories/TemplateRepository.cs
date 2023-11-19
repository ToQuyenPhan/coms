using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using LinqKit;
using System.Linq.Expressions;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class TemplateRepository : ITemplateRepository
    {
        private readonly IGenericRepository<Template> _genericRepository;

        public TemplateRepository(IGenericRepository<Template> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<PagingResult<Template>> GetTemplates(string templateName, int? contractCategoryId, 
                int? templateTypeId, int? status, int currentPage, int pageSize)
        {
            var query = await _genericRepository.WhereAsync(BuildExpression(templateName,
                    contractCategoryId, templateTypeId, status), null);
            int totalCount = query.Count();
            var list = await _genericRepository.WhereAsyncWithFilter(BuildExpression(templateName,
                    contractCategoryId, templateTypeId, status),
                    new System.Linq.Expressions.Expression<Func<Template, object>>[] { 
                            t => t.ContractCategory, t => t.TemplateTypes }, 
                    currentPage, pageSize);
            return (list.Count() > 0) ? new PagingResult<Template>(list, totalCount, currentPage, pageSize) : null;
        }

        public async Task<Template> GetTemplate(int id)
        {
            return await _genericRepository.FirstOrDefaultAsync(t => t.Id.Equals(id),
                new System.Linq.Expressions.Expression<Func<Template, object>>[]
                    {t => t.ContractCategory, t => t.TemplateTypes});
        }

        public async Task AddTemplate(Template template)
        {
            await _genericRepository.CreateAsync(template);
        }

        public async Task DeleteTemplate(Template template)
        {
            await _genericRepository.UpdateAsync(template);
        }

        private Expression<Func<Template, bool>> BuildExpression(string templateName, 
                int? contractCategoryId, int? templateTypeId, int? status)
        {
            var predicate = PredicateBuilder.New<Template>(true);
            if (!string.IsNullOrEmpty(templateName))
            {
                predicate = predicate.And(t => t.TemplateName.Contains(templateName.Trim()));
            }
            if (contractCategoryId > 0)
            {
                predicate = predicate.And(t => t.ContractCategoryId.Equals(contractCategoryId));
            }
            if (templateTypeId > 0)
            {
                predicate = predicate.And(t => t.TemplateTypeId.Equals(templateTypeId));
            }
            if (status >= 0)
            {
                predicate = predicate.And(t => t.Status.Equals((TemplateStatus)status));
            }
            return predicate;
        }
    }
}
