using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.ActionHistories;
using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class ActionHistoryRepository : IActionHistoryRepository
    {
        private readonly IGenericRepository<ActionHistory> _genericRepository;

        public ActionHistoryRepository(IGenericRepository<ActionHistory> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<ActionHistory> GetActionHistoryById(int id)
        {
            return await _genericRepository.FirstOrDefaultAsync(a => a.Id.Equals(id),
                    new System.Linq.Expressions.Expression<Func<ActionHistory, object>>[]
                    { a => a.Contract, a => a.User});
        }
        public async Task AddActionHistory(ActionHistory actionHistory)
        {
            await _genericRepository.CreateAsync(actionHistory);
        }
    }
}
