using ErrorOr;

namespace Coms.Application.Services.TemplateFields
{
    public interface ITemplateFieldService
    {
        Task<ErrorOr<IList<TemplateFieldResult>>> GetTemplateFields(int contractCategoryId,
                int partnerId, int serviceId, int templateType);
        Task<ErrorOr<IList<TemplateFieldResult>>> GetTemplateAnnexFields(int contractId, int contractCategoryId,
                int partnerId, int serviceId, int templateType);
    }
}
