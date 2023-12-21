using Coms.Application.Common.Intefaces.Persistence;
using ErrorOr;

namespace Coms.Application.Services.TemplateFields
{
    public class TemplateFieldService : ITemplateFieldService
    {
        private readonly ITemplateFieldRepository _templateFieldRepository;
        private readonly ITemplateRepository _templateRepository;
        private readonly IPartnerRepository _partnerRepository;
        private readonly ISystemSettingsRepository _systemSettingsRepository;
        private readonly IFlowRepository _flowRepository;
        private readonly IFlowDetailRepository _flowDetailRepository;
        private readonly IContractFlowDetailsRepository _userFlowDetailsRepository;
        private readonly IServiceRepository _serviceRepository;

        public TemplateFieldService(ITemplateFieldRepository templateFieldRepository,
                ITemplateRepository templateRepository,
                IPartnerRepository partnerRepository,
                ISystemSettingsRepository systemSettingsRepository,
                IFlowRepository flowRepository,
                IFlowDetailRepository flowDetailRepository,
                IContractFlowDetailsRepository userFlowDetailsRepository,
                IServiceRepository serviceRepository)
        {
            _templateFieldRepository = templateFieldRepository;
            _templateRepository = templateRepository;
            _partnerRepository = partnerRepository;
            _systemSettingsRepository = systemSettingsRepository;
            _flowRepository = flowRepository;
            _flowDetailRepository = flowDetailRepository;
            _userFlowDetailsRepository = userFlowDetailsRepository;
            _serviceRepository = serviceRepository;
        }

        public async Task<ErrorOr<IList<TemplateFieldResult>>> GetTemplateFields(int contractCategoryId, 
                int partnerId, int serviceId)
        {
            var template = await _templateRepository.GetTemplateByContractCategoryId(contractCategoryId);
            if (template is not null)
            {
                var templateFields = await _templateFieldRepository.GetTemplateFieldsByTemplateId(template.Id);
                if (templateFields is not null)
                {
                    IList<TemplateFieldResult> results = new List<TemplateFieldResult>();
                    foreach (var templateField in templateFields)
                    {
                        string? content = null;
                        bool isReadOnly = false;
                        if (templateField.FieldName.Contains("Company") || templateField.FieldName.Contains("Partner") ||
                                templateField.FieldName.Contains("Signer") || 
                                templateField.FieldName.Contains("Created Date") ||
                                templateField.FieldName.Contains("Contract Code"))
                        {
                            isReadOnly = true;
                            if (templateField.FieldName.Contains("Partner"))
                            {
                                var partner = await _partnerRepository.GetPartner(partnerId);
                                if (partner is not null)
                                {
                                    switch (templateField.FieldName)
                                    {
                                        case "Partner Name":
                                            content = partner.CompanyName;
                                            break;
                                        case "Partner Address":
                                            content = partner.Address;
                                            break;
                                        case "Partner Phone Number":
                                            content = partner.Phone;
                                            break;
                                        case "Partner Signer Name":
                                            content = partner.Representative;
                                            break;
                                        case "Partner Signer Position":
                                            content = partner.RepresentativePosition;
                                            break;
                                        case "Partner Tax Code":
                                            content = partner.TaxCode;
                                            break;
                                        case "Partner Email":
                                            content = partner.Email;
                                            break;
                                        case "Partner Signature":
                                            content = "[" + templateField.FieldName + "]";
                                            break;
                                        default: break;
                                    }
                                }
                                else
                                {
                                    return Error.NotFound("404", "Partner is not found!");
                                }
                            }
                            if (templateField.FieldName.Contains("Company"))
                            {
                                var systemSettings = await _systemSettingsRepository.GetSystemSettings();
                                if (systemSettings is not null)
                                {
                                    switch (templateField.FieldName)
                                    {
                                        case "Company Name":
                                            content = systemSettings.CompanyName;
                                            break;
                                        case "Company Address":
                                            content = systemSettings.Address;
                                            break;
                                        case "Company Phone":
                                            content = systemSettings.Phone;
                                            break;
                                        case "Company Tax Code":
                                            content = systemSettings.TaxCode;
                                            break;
                                        case "Company Hotline":
                                            content = systemSettings.Hotline;
                                            break;
                                        case "Company Email":
                                            content = systemSettings.Email;
                                            break;
                                        case "Company Signature":
                                            content = "[" + templateField.FieldName + "]";
                                            break;
                                        default: break;
                                    }
                                }
                                else
                                {
                                    return Error.NotFound("404", "The system is not have any settings!");
                                }
                            }
                            if (templateField.FieldName.Contains("Signer Name") ||
                                    templateField.FieldName.Contains("Signer Position"))
                            {
                                var flow = await _flowRepository.GetByContractCategoryId(contractCategoryId);
                                if (flow is not null)
                                {
                                    var flowDetail = await _flowDetailRepository.GetSignerByFlowId(flow.Id);
                                    switch (templateField.FieldName)
                                    {
                                        case "Signer Name":
                                            content = flowDetail.User.FullName;
                                            break;
                                        case "Signer Position":
                                            content = flowDetail.User.Position;
                                            break;
                                        default: break;
                                    }
                                }
                                else
                                {
                                    return Error.NotFound("404", "The system is not have any flows for this contract category!");
                                }
                            }
                            if (templateField.FieldName.Contains("Created Date"))
                            {
                                content = DateTime.Now.ToString("dd/MM/yyyy");
                            }
                            if (templateField.FieldName.Contains("Contract Code"))
                            {
                                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                                var stringChars = new char[8];
                                var random = new Random();
                                for (int i = 0; i < stringChars.Length; i++)
                                {
                                    stringChars[i] = chars[random.Next(chars.Length)];
                                }
                                content = new String(stringChars);
                            }
                        }
                        if (templateField.FieldName.Contains("Contract Title"))
                        {
                            content = template.TemplateName;
                        }
                        var templateFieldResult = new TemplateFieldResult()
                        {
                            Id = templateField.Id,
                            Name = templateField.FieldName,
                            IsReadOnly = isReadOnly,
                            Content = content,
                        };
                        templateFieldResult.Type = "text";
                        if (templateFieldResult.Name.Contains("Duration"))
                        {
                            templateFieldResult.Name += " (month)";
                            templateFieldResult.Type = "number";
                            templateFieldResult.MinValue = 0;
                        }
                        results.Add(templateFieldResult);
                    }
                    return results.ToList();
                }
                else
                {
                    return new List<TemplateFieldResult>();
                }
            }
            else
            {
                return Error.Failure("500", "No template is activating in this category!");
            }
        }
    }
}
