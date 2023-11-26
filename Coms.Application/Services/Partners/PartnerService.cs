using Coms.Application.Common.Intefaces.Persistence;
using ErrorOr;

namespace Coms.Application.Services.Partners
{
    public class PartnerService : IPartnerService
    {
        public readonly IPartnerRepository _partnerRepository;

        public PartnerService(IPartnerRepository partnerRepository)
        {
            _partnerRepository = partnerRepository;
        }
        public ErrorOr<IList<PartnerResult>> GetActivePartners()
        {
            if (_partnerRepository.GetActivePartners().Result is not null)
            {
                IList<PartnerResult> responses = new List<PartnerResult>();
                var results = _partnerRepository.GetActivePartners().Result;
                foreach (var partner in results)
                {
                    var response = new PartnerResult
                    {
                        Id = partner.Id,
                        Address = partner.Address,
                        Code = partner.Code,
                        CompanyName = partner.CompanyName,
                        Email = partner.Email,
                        Image = partner.Image,
                        Phone = partner.Phone,
                        Representative = partner.Representative,
                        RepresentativePosition = partner.RepresentativePosition,
                        TaxCode = partner.TaxCode,
                        Status = (int)partner.Status,
                        StatusString = partner.Status.ToString()
                    };
                    responses.Add(response);
                }
                return responses.ToList();
            }
            else
            {
                return new List<PartnerResult>();
            }
        }
    }
}
