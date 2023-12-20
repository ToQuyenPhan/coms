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

        public async Task<IList<Template>?> GetTemplates(string templateName, int? contractCategoryId, 
                int? templateTypeId, int? status, string email)
        {
            var list = await _genericRepository.WhereAsync(BuildExpression(templateName,
                    contractCategoryId, templateTypeId, status, email),
                    new System.Linq.Expressions.Expression<Func<Template, object>>[] { 
                            t => t.ContractCategory, t => t.TemplateType, t => t.User });
            return (list.Count() > 0) ? list : null;
        }

        public async Task<Template?> GetTemplate(int id)
        {
            return await _genericRepository.FirstOrDefaultAsync(t => t.Id.Equals(id),
                new System.Linq.Expressions.Expression<Func<Template, object>>[]
                    {t => t.ContractCategory, t => t.TemplateType, t => t.User});
        }

        public async Task AddTemplate(Template template)
        {
            await _genericRepository.CreateAsync(template);
        }

        public async Task UpdateTemplate(Template template)
        {
            await _genericRepository.UpdateAsync(template);
        }

        public async Task<IList<Template>?> GetActivatingTemplates()
        {
            var list = await _genericRepository.WhereAsync(t =>
                t.Status.Equals(TemplateStatus.Activating), null);
            return (list.Count() > 0) ? list.ToList() : null; 
        }

        public async Task<Template?> GetTemplateByContractCategoryId(int contractCategoryId)
        {
            return await _genericRepository.FirstOrDefaultAsync(t => t.ContractCategoryId.Equals(contractCategoryId)
                && t.Status.Equals(TemplateStatus.Activating), null);
        }

        private Expression<Func<Template, bool>> BuildExpression(string templateName, 
                int? contractCategoryId, int? templateTypeId, int? status, string email)
        {
            var predicate = PredicateBuilder.New<Template>(true);
            predicate = predicate.And(t => t.TemplateLink != "" && t.TemplateLink != null);
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
            if(!string.IsNullOrEmpty(email)) {
                predicate = predicate.And(t => t.User.Email.Contains(email.Trim()));
            }
            return predicate;
        }
    }
}
