using Coms.Application.Services.Common;
using ErrorOr;

namespace Coms.Application.Services.Partners
{
    public interface IPartnerService
    {
        ErrorOr<IList<PartnerResult>> GetActivePartners();
        Task<ErrorOr<PartnerResult>> GetPartner(int id);
        //get all partners
        ErrorOr<PagingResult<PartnerResult>> GetPartners(int? partnerId, string pepresentative, string companyName, int? status, int currentPage, int pageSize);
        //add partner
        Task<ErrorOr<PartnerResult>> AddPartnerAsync(string? image, string? representative, string? representativePosition, string? email, string? code, string? phone, string? address, string? companyName, string? taxCode,string? abbreviation);
        //delete partner by id
        Task<ErrorOr<PartnerResult>> DeletePartner(int id);
        //update partner
        Task<ErrorOr<PartnerResult>> UpdatePartner(int id, string? image, string? representative, string? representativePosition, string? email, string? code, string? phone, string? address, string? companyName, string? taxCode, string? abbreviation);
        //update partner status
        Task<ErrorOr<PartnerResult>> UpdatePartnerStatus(int id);
    }
}
