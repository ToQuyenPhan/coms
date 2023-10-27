using Coms.Application.Common.Intefaces.Authentication;
using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Common.Intefaces.Services;
using Coms.Infrastructure.Authentication;
using Coms.Infrastructure.Persistence.Context;
using Coms.Infrastructure.Persistence.Repositories;
using Coms.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coms.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, Microsoft.Extensions.Configuration.ConfigurationManager configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName)); 
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddDbContext<ComsDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ComsDB")));
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
