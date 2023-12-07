using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;
using System.Reflection.Metadata.Ecma335;

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

        public async Task<ErrorOr<PartnerResult>> GetPartner(int id)
        {
            var partner = await _partnerRepository.GetPartner(id);
            if (partner is not null)
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
                return response;
            }
            else
            {
                return Error.NotFound("404", "Partner is not found!");
            }
        }

        //add partner
        //public async Task<ErrorOr<PartnerResult>> AddPartnerAsync(AddPartnerResult partner)
        //{
            
        //    //foreach (var item in partner) for check all field is null if null return error.notfound
        //    foreach (var item in partner.GetType().GetProperties())
        //    {
        //        if (item.GetValue(partner) is null || item.GetValue(partner).Equals("string"))
        //        {
        //            return Error.NotFound("404", "Value input is null");
        //        }
        //    }

        //    var newPartner = new Partner
        //    {
        //        Address = partner.Address,
        //        Code = partner.Code,
        //        CompanyName = partner.CompanyName,
        //        Email = partner.Email,
        //        Image = partner.Image,
        //        Phone = partner.Phone,
        //        Representative = partner.Representative,
        //        RepresentativePosition = partner.RepresentativePosition,
        //        TaxCode = partner.TaxCode,
        //        Status = (PartnerStatus)(int)PartnerStatus.Active
        //    };
        //    var result = _partnerRepository.AddPartner(newPartner);
        //    if (result is not null)
        //    {
        //        return Error.NotFound("404", "Partner is not found!");
        //    }
        //    else
        //    {
        //        return Error.NotFound("404", "Partner is not found!");
        //    }
        //}
    }
}
