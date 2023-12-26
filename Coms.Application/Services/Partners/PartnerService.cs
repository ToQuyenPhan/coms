using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
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
                IList<Partner>? results = _partnerRepository.GetActivePartners().Result;
                foreach (Partner partner in results)
                {
                    PartnerResult response = new()
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
            Partner? partner = await _partnerRepository.GetPartner(id);
            if (partner is not null)
            {
                PartnerResult response = new()
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



        //get all partners
        public ErrorOr<PagingResult<PartnerResult>> GetPartners(int? partnerId, string pepresentative, string companyName, int? status, int currentPage, int pageSize)
        {
            if (_partnerRepository.GetPartners(partnerId, pepresentative, companyName, status, currentPage, pageSize).Result is not null)
            {
                IList<PartnerResult> responses = new List<PartnerResult>();
                PagingResult<Partner>? result = _partnerRepository.GetPartners(partnerId, pepresentative, companyName, status, currentPage, pageSize).Result;
                foreach (Partner partner in result.Items)
                {
                    PartnerResult partnerResult = new()
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
                    responses.Add(partnerResult);
                }
                return new
                    PagingResult<PartnerResult>(responses, result.TotalCount, result.CurrentPage,
                                       result.PageSize);
            }
            else
            {
                return new PagingResult<PartnerResult>(new List<PartnerResult>(), 0, currentPage,
                                       pageSize);
            }
        }

        public async Task<ErrorOr<PartnerResult>> AddPartnerAsync(AddPartnerResult partner)
        {

            //foreach (var item in partner) for check all field is null if null return error.notfound
            foreach (System.Reflection.PropertyInfo item in partner.GetType().GetProperties())
            {
                if (item.GetValue(partner) is null || item.GetValue(partner).Equals("string"))
                {
                    return Error.NotFound("404", "Value input is null");
                }
            }
            //check email is exist
            if (_partnerRepository.GetPartnerByEmail(partner.Email).Result is not null)
            {
                return Error.NotFound("404", "Email is exist!");
            }
            //check code is exist
            if (_partnerRepository.GetPartnerByCode(partner.Code).Result is not null)
            {
                return Error.NotFound("404", "Code is exist!");
            }
            Partner newPartner = new()
            {
                Id = 0,
                Address = partner.Address,
                Code = partner.Code,
                CompanyName = partner.CompanyName,
                Email = partner.Email,
                Image = partner.Image,
                Phone = partner.Phone,
                Representative = partner.Representative,
                RepresentativePosition = partner.RepresentativePosition,
                TaxCode = partner.TaxCode,
                Status = (PartnerStatus)(int)PartnerStatus.Active
            };
            await _partnerRepository.AddPartner(newPartner);
            Partner respone = await _partnerRepository.GetPartnerByEmail(partner.Email);
            if (respone != null)
            {
                PartnerResult response = new()
                {
                    Id = respone.Id,
                    Address = respone.Address,
                    Code = respone.Code,
                    CompanyName = respone.CompanyName,
                    Email = respone.Email,
                    Image = respone.Image,
                    Phone = respone.Phone,
                    Representative = respone.Representative,
                    RepresentativePosition = respone.RepresentativePosition,
                    TaxCode = respone.TaxCode,
                    Status = (int)respone.Status,
                    StatusString = respone.Status.ToString()
                };
                return response;
            }
            else
            {
                return Error.NotFound("404", "Partner is not found!");
            }
        }

        public async Task<ErrorOr<PartnerResult>> DeletePartner(int id)
        {
            Partner? partner = _partnerRepository.GetPartner(id).Result;
            if (partner is not null)
            {
                partner.Status = PartnerStatus.Inactive;
                await _partnerRepository.UpdatePartner(partner);
                partner = _partnerRepository.GetPartner(id).Result;
                PartnerResult response = new()
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

        public async Task<ErrorOr<PartnerResult>> UpdatePartner(int id, AddPartnerResult partner)
        {
            if (_partnerRepository.GetPartnerByEmail(partner.Email).Result is not null && _partnerRepository.GetPartnerByEmail(partner.Email).Result.Id != id)
            {
                return Error.NotFound("404", "Email is exist!");
            }
            if (_partnerRepository.GetPartnerByCode(partner.Code).Result is not null && _partnerRepository.GetPartnerByCode(partner.Code).Result.Id != id)
            {
                return Error.NotFound("404", "Code is exist!");
            }


            Partner? partnerUpdate = _partnerRepository.GetPartner(id).Result;
            if (partnerUpdate is not null)
            {
                partnerUpdate.Address = partner.Address;
                partnerUpdate.Code = partner.Code;
                partnerUpdate.CompanyName = partner.CompanyName;
                partnerUpdate.Email = partner.Email;
                partnerUpdate.Image = partner.Image;
                partnerUpdate.Phone = partner.Phone;
                partnerUpdate.Representative = partner.Representative;
                partnerUpdate.RepresentativePosition = partner.RepresentativePosition;
                partnerUpdate.TaxCode = partner.TaxCode;
                await _partnerRepository.UpdatePartner(partnerUpdate);
                partnerUpdate = _partnerRepository.GetPartner(id).Result;
                PartnerResult response = new()
                {
                    Id = partnerUpdate.Id,
                    Address = partnerUpdate.Address,
                    Code = partnerUpdate.Code,
                    CompanyName = partnerUpdate.CompanyName,
                    Email = partnerUpdate.Email,
                    Image = partnerUpdate.Image,
                    Phone = partnerUpdate.Phone,
                    Representative = partnerUpdate.Representative,
                    RepresentativePosition = partnerUpdate.RepresentativePosition,
                    TaxCode = partnerUpdate.TaxCode,
                    Status = (int)partnerUpdate.Status,
                    StatusString = partnerUpdate.Status.ToString()
                };
                return response;
            }
            else
            {
                return Error.NotFound("404", "Partner is not found!");
            }
        }

        public async Task<ErrorOr<PartnerResult>> UpdatePartnerStatus(int id)
        {
            Partner? partner = _partnerRepository.GetPartner(id).Result;
            if (partner is not null)
            {
                partner.Status = partner.Status.Equals(PartnerStatus.Active) ? PartnerStatus.Inactive : PartnerStatus.Active;
                await _partnerRepository.UpdatePartner(partner);
                partner = _partnerRepository.GetPartner(id).Result;
                PartnerResult response = new()
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
    }
}
