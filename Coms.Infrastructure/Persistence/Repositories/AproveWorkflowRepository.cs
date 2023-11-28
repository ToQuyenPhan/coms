using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class AproveWorkflowRepository : IAproveWorkflowRepository
    {
        private readonly IGenericRepository<ApproveWorkflow> _genericRepository;

        public AproveWorkflowRepository(IGenericRepository<ApproveWorkflow> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task AddUserAccess(ApproveWorkflow approveWorkflow)
        {
            await _genericRepository.CreateAsync(approveWorkflow);
        }

        public async Task UpdateApproveWorkflow(ApproveWorkflow approveWorkflow)
        {
            await _genericRepository.UpdateAsync(approveWorkflow);
        }

        public async Task<ApproveWorkflow?> GetByAccessId(int accessId)
        {
            return await _genericRepository.FirstOrDefaultAsync(wf => wf.AccessId.Equals(accessId), null);
        }
    }
}
