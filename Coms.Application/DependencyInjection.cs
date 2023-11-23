﻿using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Accesses;
using Coms.Application.Services.ActionHistories;
using Coms.Application.Services.Authentication;
using Coms.Application.Services.ContractCategories;
using Coms.Application.Services.ContractCosts;
using Coms.Application.Services.Contracts;
using Coms.Application.Services.PartnerReviews;
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
            services.AddScoped<IContractService, ContractService>();
            services.AddScoped<IAccessService, AccessService>();
            services.AddScoped<IPartnerReviewService, PartnerReviewService>();
            services.AddScoped<IActionHistoryService, ActionHistoryService>();
            services.AddScoped<IContractCostService, ContractCostService>();
            return services;
        }
    }
}
