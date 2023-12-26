using Coms.Application.Services.Common;
using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IPartnerRepository
    {
        Task<Partner?> GetPartner(int id);
        Task<IList<Partner>?> GetActivePartners();
        Task<Partner?> GetPartnerByCode(string code);
        Task<Partner?> GetPartnerByEmail(string email);
        Task<PagingResult<Partner>?> GetPartners(int? partnerId, string pepresentative, string companyName, 
                int? status, int currentPage, int pageSize);
        Task AddPartner(Partner partner);
        Task UpdatePartner(Partner partner);
    }
}
