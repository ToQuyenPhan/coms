using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IUserFlowDetailsRepository
    {
        Task<IList<User_FlowDetail>?> GetUserFlowDetailsByUserId(int userId);
    }
}
