﻿using Coms.Application.Common.Intefaces.Authentication;
using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Common.Intefaces.Services;
using Coms.Infrastructure.Authentication;
using Coms.Infrastructure.Persistence.Context;
using Coms.Infrastructure.Persistence.Repositories;
using Coms.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Coms.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
            Microsoft.Extensions.Configuration.ConfigurationManager configuration)
        {         
            services.AddAuth(configuration);
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddDbContext<ComsDBContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("ComsDB"));
                options.EnableSensitiveDataLogging();
            });
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IContractCategoryRepository, ContractCategoryRepository>();
            services.AddScoped<ITemplateRepository, TemplateRepository>();
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<IPartnerReviewRepository, PartnerReviewRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IActionHistoryRepository, ActionHistoryRepository>();
            services.AddScoped<ITemplateFileRepository, TemplateFileRepository>();
            services.AddScoped<IPartnerRepository, PartnerRepository>();
            services.AddScoped<IActionHistoryRepository, ActionHistoryRepository>();
            services.AddScoped<IContractCostRepository, ContractCostRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IContractFileRepository, ContractFileRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITemplateFieldRepository, TemplateFieldRepository>();
            services.AddScoped<IContractFlowDetailsRepository, ContractFlowDetailsRepository>();
            services.AddScoped<IFlowDetailRepository, FlowDetailRepository>();
            services.AddScoped<IContractFieldRepository, ContractFieldRepository>();
            services.AddScoped<ISystemSettingsRepository, SystemSettingRepository>();
            services.AddScoped<IFlowRepository, FlowRepository>();
            services.AddScoped<IPartnerCommentRepository, PartnerCommentRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAttachmentRepository, AttachmentRepository>();
            services.AddScoped<ILiquidationRecordRepository, LiquidationRecordRepository>();
            services.AddScoped<ILiquidationRecordFileRepository, LiquidationRecordFileRepository>();
            services.AddScoped<IContractAnnexFileRepository, ContractAnnexFileRepository>();
            services.AddScoped<IContractAnnexRepository, ContractAnnexRepository>();
            services.AddScoped<IPartnerSignRepository, PartnerSignRepository>();
			services.AddScoped<IContractAnnexAttachmentRepository, ContractAnnexAttachmentRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IContractAnnexFieldRepository, ContractAnnexFieldRepository>();
            return services;
        }

        public static IServiceCollection AddAuth(this IServiceCollection services, 
            Microsoft.Extensions.Configuration.ConfigurationManager configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.SectionName, jwtSettings);
            services.AddSingleton(Options.Create(jwtSettings));
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = 
                new Microsoft.IdentityModel.Tokens.TokenValidationParameters(){
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = 
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                });
            return services;
        }
    }
}
