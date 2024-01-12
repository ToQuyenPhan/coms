using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IContractFlowDetailsRepository
    {
        Task<Contract_FlowDetail?> GetByContractIdAndFlowDetailId(int contractId, int flowDetailId);
        Task<IList<Contract_FlowDetail>?> GetByFlowDetailId(int flowDetailId);
        Task<IList<Contract_FlowDetail>?> GetByContractId(int contractId);
        Task AddRangeContractFlowDetails(List<Contract_FlowDetail> contractFlowDetails);
        Task UpdateContractFlowDetail(Contract_FlowDetail contractFlowDetail);
        Task<IList<Contract_FlowDetail>?> GetApproversByContractId(int contractId);
        Task AddContractFlowDetail(Contract_FlowDetail contractFlowDetail);
        Task<IList<Contract_FlowDetail>?> GetByContractAnnexId(int contractAnnexId);
        Task<IList<Contract_FlowDetail>?> GetByLiquidationRecordId(int liquidationRecordId);
        Task<IList<Contract_FlowDetail>?> GetByContractIdForSign(int contractId);
        Task<IList<Contract_FlowDetail>?> GetByContractAnnexIdForSign(int contractAnnexId);
        Task<IList<Contract_FlowDetail>?> GetByLiquidationRecordForSign(int liquidationRecordId);
    }
}
