using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IFlowDetailRepository
    {
        Task<FlowDetail?> GetFlowDetail(int id);
        Task<FlowDetail?> GetSignerByFlowId(int flowId);
        Task<IList<FlowDetail>?> GetUserFlowDetailsByUserId(int userId);
        Task<IList<FlowDetail>?> GetByFlowId(int flowId);
        Task<IList<FlowDetail>?> GetUserFlowDetailsSignerByUserId(int userId);
        Task AddFlowDetail(FlowDetail flowDetail);
    }
}
