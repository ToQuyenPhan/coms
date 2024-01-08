using ErrorOr;

namespace Coms.Application.Services.ContractFields
{
    public interface IContractFieldService
    {
        Task<ErrorOr<IList<ContractFieldResult>>> GetContractFields(int contractId, int partnerId, int serviceId);
    }
}
