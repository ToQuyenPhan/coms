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
        private readonly IContractRepository _contractRepository;
        private readonly IContractAnnexRepository _contractAnnexRepository;

        public TemplateFieldService(ITemplateFieldRepository templateFieldRepository,
                ITemplateRepository templateRepository,
                IPartnerRepository partnerRepository,
                ISystemSettingsRepository systemSettingsRepository,
                IFlowRepository flowRepository,
                IFlowDetailRepository flowDetailRepository,
                IContractFlowDetailsRepository userFlowDetailsRepository,
                IServiceRepository serviceRepository,
                IContractRepository contractRepository,
                IContractAnnexRepository contractAnnexRepository)
        {
            _templateFieldRepository = templateFieldRepository;
            _templateRepository = templateRepository;
            _partnerRepository = partnerRepository;
            _systemSettingsRepository = systemSettingsRepository;
            _flowRepository = flowRepository;
            _flowDetailRepository = flowDetailRepository;
            _userFlowDetailsRepository = userFlowDetailsRepository;
            _serviceRepository = serviceRepository;
            _contractRepository = contractRepository;
            _contractAnnexRepository = contractAnnexRepository;
        }

        public async Task<ErrorOr<IList<TemplateFieldResult>>> GetTemplateFields(int contractCategoryId,
                int partnerId, int serviceId, int templateType)
        {
            var template = await _templateRepository.GetTemplateByContractCategoryIdAndTemplateType(contractCategoryId, templateType);
            if (template is not null)
            {
                var templateFields = await _templateFieldRepository.GetTemplateFieldsByTemplateId(template.Id);
                if (templateFields is not null)
                {
                    var systemSettings = await _systemSettingsRepository.GetSystemSettings();
                    var partner = await _partnerRepository.GetPartner(partnerId);
                    var flow = await _flowRepository.GetByContractCategoryId(contractCategoryId);
                    var service = await _serviceRepository.GetService(serviceId);
                    IList<TemplateFieldResult> results = new List<TemplateFieldResult>();
                    foreach (var templateField in templateFields)
                    {
                        string? content = null;
                        bool isReadOnly = false;
                        if (templateField.FieldName.Contains("Company") || templateField.FieldName.Contains("Partner") ||
                                templateField.FieldName.Contains("Signer") ||
                                templateField.FieldName.Contains("Created Date") ||
                                templateField.FieldName.Contains("Bank") || templateField.FieldName.Contains("Account") ||
                                templateField.FieldName.Contains("Service") || templateField.FieldName.Contains("Contract Code"))
                        {
                            isReadOnly = true;
                            if (templateField.FieldName.Contains("Partner"))
                            {
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
                            if (templateField.FieldName.Contains("Contract Code"))
                            {
                                string code = "HS-" + partner.Abbreviation + "/" + DateTime.Now.ToString("yy-MM-dd");
                                var contracts = await _contractRepository.GetByContractCode2(code);
                                if(contracts is not null)
                                {
                                    content = code + "/" + (contracts.Count() + 1);
                                }
                                else
                                {
                                    content = code;
                                }
                            }
                            if (templateField.FieldName.Contains("Company") ||
                                templateField.FieldName.Contains("Bank") || templateField.FieldName.Contains("Account"))
                            {
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
                                        case "Bank Account":
                                            content = systemSettings.BankAccount;
                                            break;
                                        case "Account Number":
                                            content = systemSettings.BankAccountNumber;
                                            break;
                                        case "Bank":
                                            content = systemSettings.BankName;
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
                            if (templateField.FieldName.Contains("Service"))
                            {
                                if (service is not null)
                                {
                                    switch (templateField.FieldName)
                                    {
                                        case "Service Name":
                                            content = service.ServiceName;
                                            break;
                                        case "Service Price":
                                            content = service.Price.ToString();
                                            break;
                                        default: break;
                                    }
                                }
                                else
                                {
                                    return Error.NotFound("404", "Service is not found!");
                                }
                            }
                        }
                        var templateFieldResult = new TemplateFieldResult()
                        {
                            Id = templateField.Id,
                            Name = templateField.FieldName,
                            IsReadOnly = isReadOnly,
                            Content = content,
                        };
                        templateFieldResult.Type = "text";
                        if (templateFieldResult.Name.Contains("Duration") || templateFieldResult.Name.Equals("Execution Time"))
                        {
                            templateFieldResult.Type = "number";
                            templateFieldResult.MinValue = 1;
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

        public async Task<ErrorOr<IList<TemplateFieldResult>>> GetTemplateAnnexFields(int contractId, int contractCategoryId,
                int partnerId, int serviceId, int templateType)
        {
            var template = await _templateRepository.GetTemplateByContractCategoryIdAndTemplateType(contractCategoryId, templateType);
            var contract = await _contractRepository.GetContract(contractId);
            if (template is not null)
            {
                var templateFields = await _templateFieldRepository.GetTemplateFieldsByTemplateId(template.Id);
                if (templateFields is not null)
                {
                    var systemSettings = await _systemSettingsRepository.GetSystemSettings();
                    var partner = await _partnerRepository.GetPartner(partnerId);
                    var flow = await _flowRepository.GetByContractCategoryId(contractCategoryId);
                    var service = await _serviceRepository.GetService(serviceId);
                    IList<TemplateFieldResult> results = new List<TemplateFieldResult>();
                    foreach (var templateField in templateFields)
                    {
                        string? content = null;
                        bool isReadOnly = false;
                        if (templateField.FieldName.Contains("Contract Title") || templateField.FieldName.Contains("Contract Code"))
                        {
                            isReadOnly = true;
                            if (templateField.FieldName.Contains("Contract"))
                            {
                                if (contract is not null)
                                {
                                    switch (templateField.FieldName)
                                    {
                                        case "Contract Title":
                                            content = contract.ContractName;
                                            break;
                                        case "Contract Code":
                                            content = contract.Code;
                                            break;
                                        default: break;
                                    }
                                }
                                else
                                {
                                    return Error.NotFound("404", "Contract is not found!");
                                }
                            }
                        }
                        if (templateField.FieldName.Contains("Company") || templateField.FieldName.Contains("Partner") ||
                                templateField.FieldName.Contains("Signer") ||
                                templateField.FieldName.Contains("Created Date") ||
                                templateField.FieldName.Contains("Bank") || templateField.FieldName.Contains("Account") ||
                                templateField.FieldName.Contains("Service") || templateField.FieldName.Contains("Contract Annex Code") ||
                                templateField.FieldName.Contains("Sign Date"))
                        {
                            isReadOnly = true;
                            if (templateField.FieldName.Contains("Sign Date"))
                            {
                                content = contract.CreatedDate.ToString("dd/MM/yyyy");

                            }
                            if (templateField.FieldName.Contains("Contract Annex Code"))
                            {
                                string code = "HS-" + partner.Abbreviation + "/PL/" + DateTime.Now.ToString("yy-MM-dd");
                                var contracts = await _contractAnnexRepository.GetByContractAnnexCode2(code);
                                if (contracts is not null)
                                {
                                    content = code + "/" + (contracts.Count() + 1);
                                }
                                else
                                {
                                    content = code;
                                }
                            }
                            if (templateField.FieldName.Contains("Partner"))
                            {
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
                            if (templateField.FieldName.Contains("Company") ||
                                templateField.FieldName.Contains("Bank") || templateField.FieldName.Contains("Account"))
                            {
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
                                        case "Bank Account":
                                            content = systemSettings.BankAccount;
                                            break;
                                        case "Account Number":
                                            content = systemSettings.BankAccountNumber;
                                            break;
                                        case "Bank":
                                            content = systemSettings.BankName;
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
                            if (templateField.FieldName.Contains("Service"))
                            {
                                if (service is not null)
                                {
                                    switch (templateField.FieldName)
                                    {
                                        case "Service Name":
                                            content = service.ServiceName;
                                            break;
                                        case "Service Price":
                                            content = service.Price.ToString();
                                            break;
                                        default: break;
                                    }
                                }
                                else
                                {
                                    return Error.NotFound("404", "Service is not found!");
                                }
                            }
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
                            templateFieldResult.Type = "number";
                            templateFieldResult.MinValue = 1;
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
