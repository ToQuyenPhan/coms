using Coms.Application.Services.Common;
using ErrorOr;

namespace Coms.Application.Services.Templates
{
    public interface ITemplateService
    {
        Task<ErrorOr<TemplateResult>> AddTemplate(string name, string description, int category, int type, 
                string link, int status);
        Task<ErrorOr<PagingResult<TemplateResult>>> GetTemplates(string name, int? category,
                int? type, int? status, int currentPage, int pageSize);
    }
}
