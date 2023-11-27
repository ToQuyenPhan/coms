using ErrorOr;

namespace Coms.Application.Services.Partners
{
    public interface IPartnerService
    {
        ErrorOr<IList<PartnerResult>> GetActivePartners();
        Task<ErrorOr<PartnerResult>> GetPartner(int id);
    }
}
