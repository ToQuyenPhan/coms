using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
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

        public async Task<ErrorOr<PagingResult<TemplateResult>>> GetTemplates(string name, int? category, 
                int? type, int? status, string email, int currentPage, int pageSize)
        {
            if(_templateRepository
                    .GetTemplates(name, category, type, status, email, currentPage, pageSize).Result is not null)
            {
                IList<TemplateResult> responses = new List<TemplateResult>();
                var result = _templateRepository
                    .GetTemplates(name, category, type, status, email, currentPage, pageSize).Result;
                foreach(var template in result.Items)
                {
                    var templateResult = new TemplateResult
                    {
                        Id = template.Id,
                        TemplateName = template.TemplateName,
                        Description = template.Description,
                        CreatedDate = template.CreatedDate,
                        CreatedDateString = template.CreatedDate.ToString(),
                        ContractCategoryId = template.ContractCategoryId,
                        ContractCategoryName = template.ContractCategory.CategoryName,
                        TemplateTypeId = template.TemplateTypeId,
                        TemplateTypeName = template.TemplateType.Name,
                        TemplateLink = template.TemplateLink,
                        Status = (int)template.Status,
                        StatusString = template.Status.ToString(),
                        UserId = template.User.Id,
                        UserName = template.User.Username,
                        Email = template.User.Email
                    };
                    responses.Add(templateResult);
                }
                return new 
                    PagingResult<TemplateResult>(responses, result.TotalCount, result.CurrentPage, 
                    result.PageSize);
            }
            else
            {
                return new PagingResult<TemplateResult>(new List<TemplateResult>(), 0, currentPage,
                    pageSize);
            }
        }

        public async Task<ErrorOr<TemplateResult>> AddTemplate(string name, string description, int category, int type,
                string link, int status, int userId)
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
                    Status = (TemplateStatus) status,
                    UserId = userId
                };
                await _templateRepository.AddTemplate(template);
                var createdTemplate = await _templateRepository.GetTemplate(template.Id);
                var templateResult = new TemplateResult
                {
                    Id = createdTemplate.Id,
                    TemplateName = createdTemplate.TemplateName,
                    Description = createdTemplate.Description,
                    CreatedDate = createdTemplate.CreatedDate,
                    CreatedDateString = createdTemplate.CreatedDate.ToString(),
                    ContractCategoryId= createdTemplate.ContractCategory.Id,
                    ContractCategoryName = createdTemplate.ContractCategory.CategoryName,
                    TemplateTypeId = createdTemplate.TemplateType.Id,
                    TemplateTypeName = createdTemplate.TemplateType.Name,
                    TemplateLink = createdTemplate.TemplateLink,
                    Status = (int)createdTemplate.Status,
                    StatusString = createdTemplate.Status.ToString(),
                    UserId = createdTemplate.User.Id,
                    UserName = createdTemplate.User.Username,
                    Email = createdTemplate.User.Email
                };
                return templateResult;
            }
            catch(Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<TemplateResult>> DeleteTemplate(int id)
        {
            try
            {
                if(_templateRepository.GetTemplate(id).Result is not null)
                {
                    var template = await _templateRepository.GetTemplate(id);
                    template.Status = TemplateStatus.Deleted;
                    await _templateRepository.UpdateTemplate(template);
                    var templateResult = new TemplateResult
                    {
                        Id = template.Id,
                        TemplateName = template.TemplateName,
                        Description = template.Description,
                        CreatedDate = template.CreatedDate,
                        CreatedDateString = template.CreatedDate.ToString(),
                        ContractCategoryId = template.ContractCategoryId,
                        ContractCategoryName = template.ContractCategory.CategoryName,
                        TemplateTypeId = template.TemplateTypeId,
                        TemplateTypeName = template.TemplateType.Name,
                        TemplateLink = template.TemplateLink,
                        Status = (int)template.Status,
                        StatusString = template.Status.ToString(),
                        UserId = template.User.Id,
                        UserName = template.User.Username,
                        Email = template.User.Email
                    };
                    return templateResult;
                }
                else
                {
                    return Error.NotFound();
                } 
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }
    }
}
