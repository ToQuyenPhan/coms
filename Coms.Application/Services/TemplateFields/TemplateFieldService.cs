using Coms.Application.Common.Intefaces.Persistence;
using ErrorOr;

namespace Coms.Application.Services.TemplateFields
{
    public class TemplateFieldService : ITemplateFieldService
    {
        private readonly ITemplateFieldRepository _templateFieldRepository;
        private readonly ITemplateRepository _templateRepository;
        private readonly IPartnerRepository _partnerRepository;

        public TemplateFieldService(ITemplateFieldRepository templateFieldRepository,
                ITemplateRepository templateRepository,
                IPartnerRepository partnerRepository)
        {
            _templateFieldRepository = templateFieldRepository;
            _templateRepository = templateRepository;
            _partnerRepository = partnerRepository;
        }

        public async Task<ErrorOr<IList<TemplateFieldResult>>> GetTemplateFields(int contractCategoryId, int partnerId)
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
                        if(templateField.FieldName.Contains("Company") || templateField.FieldName.Contains("Partner") ||
                                templateField.FieldName.Contains("Signer"))
                        {
                            isReadOnly = true;
                            if (templateField.FieldName.Contains("Partner"))
                            {
                                var partner = await _partnerRepository.GetPartner(partnerId);
                                if (partner is not null) {
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
                                        default: break;
                                    }
                                }
                                else
                                {
                                    return Error.NotFound("404", "Partner is not found!");
                                }
                            }
                        }
                        if (templateField.FieldName.Contains("Signature"))
                        {
                            continue;
                        }
                        var templateFieldResult = new TemplateFieldResult()
                        {
                            Id = templateField.Id,
                            Name = templateField.FieldName,
                            IsReadOnly = isReadOnly,
                            Content = content,
                        };
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
