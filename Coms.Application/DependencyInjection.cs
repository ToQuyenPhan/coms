using Coms.Application.Services.Authentication;
using Coms.Application.Services.ContractCategories;
using Coms.Application.Services.Templates;
using Coms.Application.Services.TemplateTypes;
using Microsoft.Extensions.DependencyInjection;

namespace Coms.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IContractCategoryService, ContractCategoryService>();
            services.AddScoped<ITemplateTypeService, TemplateTypeService>();
            services.AddScoped<ITemplateService, TemplateService>();
            return services;
        }
    }
}
