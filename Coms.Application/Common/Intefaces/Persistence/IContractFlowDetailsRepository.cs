using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IContractFlowDetailsRepository
    {
        Task<Contract_FlowDetail?> GetByContractIdAndFlowDetailId(int contractId, int flowDetailId);
        Task<IList<Contract_FlowDetail>?> GetByFlowDetailId(int flowDetailId);
        Task<IList<Contract_FlowDetail>?> GetByContractId(int contractId);
    }
}
