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
        Task<ErrorOr<PartnerResult>> AddPartnerAsync(AddPartnerResult partner);
        //delete partner by id
        Task<ErrorOr<PartnerResult>> DeletePartner(int id);
        //update partner
        Task<ErrorOr<PartnerResult>> UpdatePartner(int id, AddPartnerResult partner);
        //update partner status
        Task<ErrorOr<PartnerResult>> UpdatePartnerStatus(int id);
    }
}
