using ErrorOr;

namespace Coms.Application.Services.TemplateTypes
{
    public interface ITemplateTypeService
    {
        ErrorOr<IList<TemplateTypeResult>> GetAllTemplateTypes();
    }
}
