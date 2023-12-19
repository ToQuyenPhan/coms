using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IUserFlowDetailsRepository
    {
        Task<IList<User_FlowDetail>?> GetUserFlowDetailsByUserId(int userId);
        Task<IList<User_FlowDetail>?> GetUserFlowDetailsByContractId(int contractId);
        Task<IList<User_FlowDetail>?> GetUserFlowDetailsByUserIdAndContractId(int userId, int contractId);
        Task<User_FlowDetail?> GetByFlowDetailId(int flowDetailId);
    }
}
