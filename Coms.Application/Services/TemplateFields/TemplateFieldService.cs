using Coms.Application.Common.Intefaces.Persistence;
using ErrorOr;

namespace Coms.Application.Services.TemplateFields
{
    public class TemplateFieldService : ITemplateFieldService
    {
        private readonly ITemplateFieldRepository _templateFieldRepository;
        private readonly ITemplateRepository _templateRepository;

        public TemplateFieldService(ITemplateFieldRepository templateFieldRepository, 
                ITemplateRepository templateRepository)
        {
            _templateFieldRepository = templateFieldRepository;
            _templateRepository = templateRepository;
        }

        public async Task<ErrorOr<IList<TemplateFieldResult>>> GetTemplateFields(int contractCategoryId)
        {
            var template = await _templateRepository.GetTemplateByContractCategoryId(contractCategoryId);
            if (template is not null)
            {
                var templateFields = await _templateFieldRepository.GetTemplateFieldsByTemplateId(template.Id);
                if (templateFields is not null)
                {
                    IList<TemplateFieldResult> results = new List<TemplateFieldResult>();
                    foreach (var templateField in templateFields)
                    {
                        var templateFieldResult = new TemplateFieldResult()
                        {
                            Id = templateField.Id,
                            Name = templateField.FieldName
                        };
                        results.Add(templateFieldResult);
                    }
                    return results.ToList();
                }
                else
                {
                    return new List<TemplateFieldResult>();
                }
            }
            else
            {
                return Error.Failure("500", "No template is activating in this category!");
            }
        }
    }
}
