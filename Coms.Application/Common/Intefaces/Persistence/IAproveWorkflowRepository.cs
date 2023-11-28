using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IAproveWorkflowRepository
    {
        Task AddUserAccess(ApproveWorkflow approveWorkflow);
        Task UpdateApproveWorkflow(ApproveWorkflow approveWorkflow);
        Task<ApproveWorkflow?> GetByAccessId(int accessId);
    }
}
