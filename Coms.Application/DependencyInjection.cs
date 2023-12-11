using Coms.Application.Services.ActionHistories;
using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Accesses;
using Coms.Application.Services.ActionHistories;
using Coms.Application.Services.Authentication;
using Coms.Application.Services.Comments;
using Coms.Application.Services.ContractCategories;
using Coms.Application.Services.ContractCosts;
using Coms.Application.Services.Contracts;
using Coms.Application.Services.PartnerReviews;
using Coms.Application.Services.TemplateFiles;
using Coms.Application.Services.Templates;
using Coms.Application.Services.TemplateTypes;
using Microsoft.Extensions.DependencyInjection;
using Coms.Application.Services.Services;
using Coms.Application.Services.ContractFiles;
using Coms.Application.Services.Partners;
using Coms.Application.Services.Users;
using Coms.Application.Services.UserAccesses;
using Coms.Application.Services.Flows;

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
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IActionHistoryService, ActionHistoryService>();
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddScoped<ITemplateFileService, TemplateFileService>();
            services.AddScoped<IAccessService, AccessService>();
            services.AddScoped<IPartnerReviewService, PartnerReviewService>();
            services.AddScoped<IActionHistoryService, ActionHistoryService>();
            services.AddScoped<IContractCostService, ContractCostService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IContractFileService, ContractFileService>();
            services.AddScoped<IPartnerService, PartnerService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserAccessService, UserAccessService>();
            services.AddScoped<IFlowService, FlowService>();
            return services;
        }
    }
}
