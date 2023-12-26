using Coms.Application.Services.Common;
using ErrorOr;

namespace Coms.Application.Services.Partners
{
    public interface IPartnerService
    {
        ErrorOr<IList<PartnerResult>> GetActivePartners();
        Task<ErrorOr<PartnerResult>> GetPartner(int id);
        ErrorOr<PagingResult<PartnerResult>> GetPartners(int? partnerId, string pepresentative, string companyName, 
                int? status, int currentPage, int pageSize);
        Task<ErrorOr<PartnerResult>> AddPartnerAsync(AddPartnerResult partner);
        Task<ErrorOr<PartnerResult>> DeletePartner(int id);
        Task<ErrorOr<PartnerResult>> UpdatePartner(int id, AddPartnerResult partner);
        Task<ErrorOr<PartnerResult>> UpdatePartnerStatus(int id);
    }
}
