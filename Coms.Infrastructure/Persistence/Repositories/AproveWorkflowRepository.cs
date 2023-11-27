using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
