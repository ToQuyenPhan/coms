using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.ActionHistories;
using Coms.Application.Services.Common;
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
                ah.ActionType.Equals(ActionType.Created), new System.Linq.Expressions.Expression<Func<ActionHistory, object>>[] { ah => ah.User,
                        ah => ah.Contract });
            return (list.Count() > 0) ? list : null;
        }

        public async Task<IList<ActionHistory>> GetCommentActionByContractId(int contractId, int userId)
        {
            var list = await _genericRepository.WhereAsync(ah => ah.ContractId.Equals(contractId) &&
                ah.ActionType.Equals(ActionType.Commented) && !ah.UserId.Equals(userId), null);
            return (list.Count() > 0) ? list : null;
        }

        public async Task<IList<ActionHistory>> GetOtherUserActionByContractId(int contractId, int userId)
        {
            var list = await _genericRepository.WhereAsync(ah => ah.ContractId.Equals(contractId) &&
                !ah.UserId.Equals(userId), new System.Linq.Expressions.Expression<Func<ActionHistory, object>>[] { ah => ah.User,
                        ah => ah.Contract });
            list = list.OrderByDescending(ah => ah.CreatedAt).ToList();
            return (list.Count() > 0) ? list : null;
        }

        //add get all comments by contract id
        public async Task<IList<ActionHistory>> GetCreateActionByContractId(int contractId)
        {
            var list = await _genericRepository.WhereAsync(ah => ah.ContractId.Equals(contractId) &&
                           ah.ActionType.Equals(ActionType.Created), new System.Linq.Expressions.Expression<Func<ActionHistory, object>>[] { ah => ah.User,
                                                  ah => ah.Contract });
            return (list.Count() > 0) ? list : null;
        }                                                                   
    }
}
