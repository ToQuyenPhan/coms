using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class FlowRepository : IFlowRepository
    {
        private readonly IGenericRepository<Flow> _genericRepository;

        public FlowRepository(IGenericRepository<Flow> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<Flow> GetFlowById(int id)
        {
            return await _genericRepository.FirstOrDefaultAsync(a => a.Id.Equals(id),
                    new System.Linq.Expressions.Expression<Func<Flow, object>>[]
                    { a => a.ContractCategory});
        }

        public async Task AddFlow(Flow flow)
        {
            await _genericRepository.CreateAsync(flow);
        }
    }
}
