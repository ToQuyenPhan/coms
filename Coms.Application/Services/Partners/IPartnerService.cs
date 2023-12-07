using Coms.Domain.Entities;
using ErrorOr;

namespace Coms.Application.Services.Partners
{
    public interface IPartnerService
    {
        ErrorOr<IList<PartnerResult>> GetActivePartners();
        Task<ErrorOr<PartnerResult>> GetPartner(int id);
        //add partner
        //ErrorOr<PartnerResult> AddPartnerAsync(AddPartnerResult command);
    }
}
