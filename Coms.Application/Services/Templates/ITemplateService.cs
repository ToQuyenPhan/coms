using ErrorOr;

namespace Coms.Application.Services.Templates
{
    public interface ITemplateService
    {
        Task<ErrorOr<TemplateResult>> AddTemplate(string name, string description, int category, int type, 
                string link, int status);
    }
}
