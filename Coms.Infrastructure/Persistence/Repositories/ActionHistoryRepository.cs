using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using Coms.Domain.Enum;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class ActionHistoryRepository : IActionHistoryRepository
    {
        private readonly IGenericRepository<ActionHistory> _genericRepository;

        public ActionHistoryRepository(IGenericRepository<ActionHistory> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<IList<ActionHistory>> GetCreateActionByUserId(int userId)
        {
            var list = await _genericRepository.WhereAsync(ah => ah.UserId.Equals(userId) &&
                ah.ActionType.Equals(ActionType.Created), null);
            return (list.Count() > 0) ? list : null;
        }

        public async Task<IList<ActionHistory>> GetCommentActionByContractId(int contractId, int userId)
        {
            var list = await _genericRepository.WhereAsync(ah => ah.ContractId.Equals(contractId) &&
                ah.ActionType.Equals(ActionType.Commented) && !ah.UserId.Equals(userId), null);
            return (list.Count() > 0) ? list : null;
        }
    }
}
