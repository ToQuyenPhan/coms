using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;

namespace Coms.Application.Services.Templates
{
    public class TemplateService : ITemplateService
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IContractCategoryRepository _contractCategoryRepository;
        private readonly ITemplateTypeRepository _templateTypeRepository;

        public TemplateService(ITemplateRepository templateRepository, 
                IContractCategoryRepository contractCategoryRepository,
                ITemplateTypeRepository templateTypeRepository)
        {
            _templateRepository = templateRepository;
            _contractCategoryRepository = contractCategoryRepository;
            _templateTypeRepository = templateTypeRepository;
        }

        public async Task<ErrorOr<TemplateResult>> AddTemplate(string name, string description, int category, int type,
                string link, int status)
        {
            try
            {
                var template = new Template
                {
                    TemplateName = name,
                    Description = description,
                    ContractCategoryId = category,
                    TemplateTypeId = type,
                    TemplateLink = link,
                    CreatedDate = DateTime.Now,
                    Status = (TemplateStatus) status
                };
                await _templateRepository.AddTemplate(template);
                var contractCategory = 
                        _contractCategoryRepository.GetActiveContractCategoryById(category).Result;
                var templateType = _templateTypeRepository.GetTemplateTypeById(type).Result;
                var templateResult = new TemplateResult
                {
                    Id = template.Id,
                    TemplateName = name,
                    Description = description,
                    CreatedDate = template.CreatedDate,
                    CreatedDateString = template.CreatedDate.ToString(),
                    ContractCategoryId= category,
                    ContractCategoryName = contractCategory.CategoryName,
                    TemplateTypeId = type,
                    TemplateTypeName = templateType.Name,
                    TemplateLink = link,
                    Status = status,
                    StatusString = template.Status.ToString(),
                };
                return templateResult;
            }
            catch(Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }
    }
}
