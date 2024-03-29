﻿using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using Coms.Domain.Enum;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class FlowDetailRepository : IFlowDetailRepository
    {
        private readonly IGenericRepository<FlowDetail> _genericRepository;

        public FlowDetailRepository(IGenericRepository<FlowDetail> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<FlowDetail?> GetFlowDetail(int id)
        {
            return await _genericRepository.FirstOrDefaultAsync(fd => fd.Id == id, 
                    new System.Linq.Expressions.Expression<Func<FlowDetail, object>>[] { fd => fd.User, fd => fd.Flow });
        }

        public async Task<FlowDetail?> GetSignerByFlowId(int flowId)
        {
            return await _genericRepository.FirstOrDefaultAsync(fd => fd.FlowID == flowId && 
                fd.FlowRole.Equals(FlowRole.Signer), new System.Linq.Expressions.Expression<Func<FlowDetail, object>>[] { fd => fd.User, fd => fd.Flow });
        }

        public async Task<IList<FlowDetail>?> GetUserFlowDetailsByUserId(int userId)
        {
            var list = await _genericRepository.WhereAsync(fd => fd.UserId.Equals(userId),
                new System.Linq.Expressions.Expression<Func<FlowDetail, object>>[] { fd => fd.User, fd => fd.Flow });
            return (list.Count() > 0) ? list : null;
        }

        public async Task<IList<FlowDetail>?> GetByFlowId(int flowId)
        {
            var list = await _genericRepository.WhereAsync(fd => fd.FlowID.Equals(flowId),
                    new System.Linq.Expressions.Expression<Func<FlowDetail, object>>[] { fd => fd.User, fd => fd.Flow });
            return (list.Count() > 0) ? list : null;
        }

        public async Task<IList<FlowDetail>?> GetUserFlowDetailsSignerByUserId(int userId)
        {
            var list = await _genericRepository.WhereAsync(fd => fd.UserId.Equals(userId) && (int)fd.FlowRole == 1,
                new System.Linq.Expressions.Expression<Func<FlowDetail, object>>[] { fd => fd.User, fd => fd.Flow });
            return (list.Count() > 0) ? list : null;
        }
        public async Task AddFlowDetail(FlowDetail flowDetail)
        {
            await _genericRepository.CreateAsync(flowDetail);
        }

        public async Task UpdateFlowDetail(FlowDetail flowDetail)
        {
            await _genericRepository.UpdateAsync(flowDetail);
        }
    }
}
