using Coms.Application.Services.Common;
using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IPartnerRepository
    {
        Task<Partner?> GetPartner(int id);
        Task<IList<Partner>?> GetActivePartners();
        Task<Partner?> GetPartnerByCode(string code);
        //get partner by email
        Task<Partner?> GetPartnerByEmail(string email);
        //get all partners
        Task<PagingResult<Partner>?> GetPartners(int? partnerId, string pepresentative, string companyName, int? status, int currentPage, int pageSize);

        //add partner
        Task AddPartner(Partner partner);
        //update partner
        Task UpdatePartner(Partner partner);
    }
}
