using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.ContractCategories;
using ErrorOr;

namespace Coms.Application.Services.TemplateTypes
{
    public class TemplateTypeService : ITemplateTypeService
    {
        private readonly ITemplateTypeRepository _templateTypeRepository;

        public TemplateTypeService(ITemplateTypeRepository templateTypeRepository)
        {
            _templateTypeRepository = templateTypeRepository;
        }

        public ErrorOr<IList<TemplateTypeResult>> GetAllTemplateTypes()
        {
            if (_templateTypeRepository.GetAllTemplateTypes().Result is not null)
            {
                IList<TemplateTypeResult> responses = new List<TemplateTypeResult>();
                var results = _templateTypeRepository.GetAllTemplateTypes().Result;
                foreach (var templateType in results)
                {
                    var response = new TemplateTypeResult
                    {
                        Id = templateType.Id,
                        Name = templateType.Name
                    };
                    responses.Add(response);
                }
                return responses.ToList();
            }
            else
            {
                return new List<TemplateTypeResult>();
            }
        }
    }
}
