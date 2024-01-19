using Coms.Application.Services.ActionHistories;
using Coms.Application.Services.Authentication;
using Coms.Application.Services.Comments;
using Coms.Application.Services.ContractCategories;
using Coms.Application.Services.ContractCosts;
using Coms.Application.Services.Contracts;
using Coms.Application.Services.PartnerReviews;
using Coms.Application.Services.TemplateFiles;
using Coms.Application.Services.Templates;
using Microsoft.Extensions.DependencyInjection;
using Coms.Application.Services.Services;
using Coms.Application.Services.ContractFiles;
using Coms.Application.Services.Partners;
using Coms.Application.Services.Users;
using Coms.Application.Services.UserFlowDetails;
using Coms.Application.Services.TemplateFields;
using Coms.Application.Services.Attachments;
using Coms.Application.Services.PartnerComments;
using Coms.Application.Services.Documents;
using Coms.Application.Services.Signs;
using Coms.Application.Services.ContractAnnexes;
using Coms.Application.Services.ContractFields;
using Coms.Application.Services.Coordinates;
using Coms.Application.Services.Flows;
using Coms.Application.Services.FlowDetails;
using Coms.Application.Services.LiquidationRecords;
using Coms.Application.Services.Schedules;
using Coms.Application.Services.ContractAnnexFields;
using Coms.Application.Services.SystemSettingService;
using Coms.Application.Services.ContractAnnexFiles;

namespace Coms.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IContractCategoryService, ContractCategoryService>();
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<IContractService, ContractService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IActionHistoryService, ActionHistoryService>();
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddScoped<ITemplateFileService, TemplateFileService>();
            services.AddScoped<IPartnerReviewService, PartnerReviewService>();
            services.AddScoped<IActionHistoryService, ActionHistoryService>();
            services.AddScoped<IContractCostService, ContractCostService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IContractFileService, ContractFileService>();
            services.AddScoped<IPartnerService, PartnerService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserFlowDetailService, UserFlowDetailService>();
            services.AddScoped<ITemplateFieldService, TemplateFieldService>();
            services.AddScoped<IPartnerCommentService, PartnerCommentService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<ISignService, SignService>();
            services.AddScoped<IContractAnnexesService, ContractAnnexesService>();
            services.AddScoped<IContractFieldService, ContractFieldService>();
            services.AddScoped<ICoordinateService, CoordinateService>();
            services.AddScoped<IFlowDetailService, FlowDetailService>();
            services.AddScoped<IFlowService, FlowService>();
            services.AddScoped<ILiquidationRecordsService, LiquidationRecordsService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IContractAnnexFieldService, ContractAnnexFieldService>();
            services.AddScoped<ISystemSettingService, SystemSettingService>();
            services.AddScoped<IContractAnnexFileService, ContractAnnexFileService>();
            return services;
        }
    }
}
