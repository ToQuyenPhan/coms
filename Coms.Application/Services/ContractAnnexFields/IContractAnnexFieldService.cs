using ErrorOr;

namespace Coms.Application.Services.ContractAnnexFields
{
    public interface IContractAnnexFieldService
    {
        Task<ErrorOr<IList<ContractAnnexFieldResult>>> GetContractAnnexFields(int contractAnnexId, int contractId, int partnerId, int serviceId);
    }
}
