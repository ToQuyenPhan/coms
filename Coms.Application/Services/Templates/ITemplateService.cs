using Coms.Application.Services.Common;
using ErrorOr;

namespace Coms.Application.Services.Templates
{
    public interface ITemplateService
    {
        Task<ErrorOr<TemplateResult>> AddTemplate(string name, string description, int category, int type, 
                int status, int userId);
        Task<ErrorOr<PagingResult<TemplateResult>>> GetTemplates(string name, int? category,
                int? type, int? status, string email, int currentPage, int pageSize);
        Task<ErrorOr<TemplateResult>> DeleteTemplate(int id);
        Task<ErrorOr<TemplateSfdtResult>> GetTemplate(int templateId);
        Task<ErrorOr<TemplateResult>> UpdateTemplate(string name, string description, int category,
                int type, int status, int templateId);
    }
}
