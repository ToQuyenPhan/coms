using Coms.Application.Services.Authentication;
using Coms.Application.Services.ContractCategories;
using Microsoft.Extensions.DependencyInjection;

namespace Coms.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IContractCategoryService, ContractCategoryService>();
            return services;
        }
    }
}
