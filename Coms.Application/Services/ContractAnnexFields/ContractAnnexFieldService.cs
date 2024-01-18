using Coms.Application.Common.Intefaces.Persistence;
using ErrorOr;

namespace Coms.Application.Services.ContractAnnexFields
{
    public class ContractAnnexFieldService : IContractAnnexFieldService
    {
        private readonly IContractAnnexFieldRepository _contractAnnexFieldRepository;
        private readonly ISystemSettingsRepository _systemSettingsRepository;
        private readonly IPartnerRepository _partnerRepository;
        private readonly IServiceRepository _serviceRepository;

        public ContractAnnexFieldService(IContractAnnexFieldRepository contractAnnexFieldRepository, 
                ISystemSettingsRepository systemSettingsRepository, IPartnerRepository partnerRepository,
                IServiceRepository serviceRepository)
        {
            _contractAnnexFieldRepository = contractAnnexFieldRepository;
            _systemSettingsRepository = systemSettingsRepository;
            _partnerRepository = partnerRepository;
            _serviceRepository = serviceRepository;
        }

        public async Task<ErrorOr<IList<ContractAnnexFieldResult>>> GetContractAnnexFields(int contractAnnexId, int contractId, int partnerId, int serviceId)
        {
            var contractFields = await _contractAnnexFieldRepository.GetByContractAnnexId(contractAnnexId);
            //stop
            if (contractFields is not null)
            {
                var systemSettings = await _systemSettingsRepository.GetSystemSettings();
                var partner = await _partnerRepository.GetPartner(partnerId);
                var service = await _serviceRepository.GetService(serviceId);
                IList<ContractAnnexFieldResult> results = new List<ContractAnnexFieldResult>();
                foreach (var contractField in contractFields)
                {
                    string content = contractField.Content;
                    bool isReadOnly = false;
                    if (contractField.FieldName.Contains("Company") || contractField.FieldName.Contains("Partner") ||
                            contractField.FieldName.Contains("Signer") || contractField.FieldName.Contains("Contract Code") ||
                            contractField.FieldName.Contains("Created Date") ||
                            contractField.FieldName.Contains("Bank") || contractField.FieldName.Contains("Account") ||
                            contractField.FieldName.Contains("Service"))
                    {
                        isReadOnly = true;
                        if (contractField.FieldName.Contains("Partner"))
                        {
                            if (partner is not null)
                            {
                                switch (contractField.FieldName)
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
                                        content = "[" + contractField.FieldName + "]";
                                        break;
                                    default: break;
                                }
                            }
                            else
                            {
                                return Error.NotFound("404", "Partner is not found!");
                            }
                        }
                        if (contractField.FieldName.Contains("Company") ||
                            contractField.FieldName.Contains("Bank") || contractField.FieldName.Contains("Account"))
                        {
                            if (systemSettings is not null)
                            {
                                switch (contractField.FieldName)
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
                                        content = "[" + contractField.FieldName + "]";
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
                        //if (contractField.FieldName.Contains("Signer Name") ||
                        //        contractField.FieldName.Contains("Signer Position"))
                        //{
                        //    switch (contractField.FieldName)
                        //    {
                        //        case "Signer Name":
                        //            content = contractField.Content;
                        //            break;
                        //        case "Signer Position":
                        //            content = contractField.Content;
                        //            break;
                        //        default: break;
                        //    }
                        //}
                        //if (contractField.FieldName.Contains("Created Date"))
                        //{
                        //    content = contractField.Content;
                        //}
                        if (contractField.FieldName.Contains("Service"))
                        {
                            if (service is not null)
                            {
                                switch (contractField.FieldName)
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
                    //if (contractField.FieldName.Contains("Contract Title"))
                    //{
                    //    content = contractField.Content;
                    //}
                    var contractAnnexFieldResult = new ContractAnnexFieldResult()
                    {
                        Id = contractField.Id,
                        Name = contractField.FieldName,
                        IsReadOnly = isReadOnly,
                        Content = content,
                    };
                    contractAnnexFieldResult.Type = "text";
                    if (contractAnnexFieldResult.Name.Contains("Duration"))
                    {
                        contractAnnexFieldResult.Type = "number";
                        contractAnnexFieldResult.MinValue = 0;
                    }
                    results.Add(contractAnnexFieldResult);
                }
                return results.ToList();
            }
            else
            {
                return Error.Failure("500", "Contract Annex fields not found!");
            }
        }
    }
}
