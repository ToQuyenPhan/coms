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
                        Abbreviation = partner.Abbreviation,
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
                    Abbreviation = partner.Abbreviation,
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
                        Abbreviation = partner.Abbreviation,
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


        //add partner
        public async Task<ErrorOr<PartnerResult>> AddPartnerAsync(string? image, string? representative, string? representativePosition, string? email, string? code, string? phone, string? address, string? companyName, string? taxCode, string? abbreviation)
        {
            //check email is exist
            if (_partnerRepository.GetPartnerByEmail(email).Result is not null)
            {
                return Error.NotFound("404", "Email is exist!");
            }
            //check code is exist
            if (_partnerRepository.GetPartnerByCode(code).Result is not null)
            {
                return Error.NotFound("404", "Code is exist!");
            }

            Partner newPartner = new()
            {
                Id = 0,
                Address = address,
                Code = code,
                CompanyName = companyName,
                Email = email,
                Image = image,
                Phone = phone,
                Representative = representative,
                RepresentativePosition = representativePosition,
                Abbreviation= abbreviation,
                TaxCode = taxCode,
                Status = (PartnerStatus)(int)PartnerStatus.Active
            };
            await _partnerRepository.AddPartner(newPartner);
            Partner respone = await _partnerRepository.GetPartnerByEmail(email);
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
                    Abbreviation = respone.Abbreviation,
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

        //delete partner by id
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
                    Abbreviation = partner.Abbreviation,
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

        //update partner with string? image, string? representative, string? representativePosition, string? email, string? code, string? phone, string? address, string? companyName, string? taxCode not use addPartnerRequest
        public async Task<ErrorOr<PartnerResult>> UpdatePartner(int id, string? image, string? representative, string? representativePosition, string? email, string? code, string? phone, string? address, string? companyName, string? taxCode, string? abbreviation)
        {
            //check email is exist
            if (_partnerRepository.GetPartnerByEmail(email).Result is not null && _partnerRepository.GetPartnerByEmail(email).Result.Id != id)
            {
                return Error.NotFound("404", "Email is exist!");
            }
            //check code is exist
            if (_partnerRepository.GetPartnerByCode(code).Result is not null && _partnerRepository.GetPartnerByCode(code).Result.Id != id)
            {
                return Error.NotFound("404", "Code is exist!");
            }

            Partner? partnerUpdate = _partnerRepository.GetPartner(id).Result;
            if (partnerUpdate is not null)
            {
                partnerUpdate.Address = address;
                partnerUpdate.Code = code;
                partnerUpdate.CompanyName = companyName;
                partnerUpdate.Email = email;
                partnerUpdate.Image = image;
                partnerUpdate.Phone = phone;
                partnerUpdate.Representative = representative;
                partnerUpdate.RepresentativePosition = representativePosition;
                partnerUpdate.Abbreviation = abbreviation;
                partnerUpdate.TaxCode = taxCode;
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
                    Abbreviation = partnerUpdate.Abbreviation,
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

        //update partner status check status if active change to inactive and vice versa
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
                    Abbreviation = partner.Abbreviation,
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
