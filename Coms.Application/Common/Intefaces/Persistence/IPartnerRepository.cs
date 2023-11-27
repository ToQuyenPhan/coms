using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IPartnerRepository
    {
        Task<Partner?> GetPartner(int id);
        Task<IList<Partner>?> GetActivePartners();
        Task<Partner?> GetPartnerByCode(string code);
    }
}
