﻿using Coms.Application.Common.Intefaces.Persistence;
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

        public async Task<IList<ActionHistory>?> GetCreateActionByUserId(int userId)
        {
            var list = await _genericRepository.WhereAsync(ah => ah.UserId.Equals(userId) &&
                ah.ActionType.Equals(ActionType.Created) && ah.ContractId != null, new System.Linq.Expressions.Expression<Func<ActionHistory, object>>[] { ah => ah.User,
                        ah => ah.Contract });
            return (list.Count() > 0) ? list : null;
        }

        public async Task<IList<ActionHistory>?> GetCommentActionByContractId(int contractId, int userId)
        {
            var list = await _genericRepository.WhereAsync(ah => ah.ContractId.Equals(contractId) &&
                ah.ActionType.Equals(ActionType.Commented) && !ah.UserId.Equals(userId) 
                && !ah.Contract.Status.Equals(DocumentStatus.Deleted) && !ah.Contract.Status.Equals(DocumentStatus.Edited), 
                new System.Linq.Expressions.Expression<Func<ActionHistory, object>>[] { ah => ah.User, ah => ah.Contract });
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

        public async Task<ActionHistory?> GetCreateActionByContractId(int contractId)
        {
           return await _genericRepository.FirstOrDefaultAsync(ah => ah.ContractId.Equals(contractId) &&
                           ah.ActionType.Equals(ActionType.Created), 
                           new System.Linq.Expressions.Expression<Func<ActionHistory, object>>[] { ah => ah.User,
                           ah => ah.Contract });
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

        public async Task<IList<ActionHistory>?> GetCommentActionByContractId(int contractId)
        {
            var list = await _genericRepository.WhereAsync(ah => ah.ContractId.Equals(contractId) &&
                           ah.ActionType.Equals(ActionType.Commented), new System.Linq.Expressions.Expression<Func<ActionHistory, object>>[] { ah => ah.User,
                                                  ah => ah.Contract, ah => ah.User});
            return (list.Count() > 0) ? list : null;
        }

        public async Task UpdateActionHistory(ActionHistory actionHistory)
        {
            await _genericRepository.UpdateAsync(actionHistory);
        }

        public async Task<IList<ActionHistory>?> GetByContractId(int contractId)
        {
            var list = await _genericRepository.WhereAsync(ah => ah.ContractId.Equals(contractId),
                    new System.Linq.Expressions.Expression<Func<ActionHistory, object>>[]
                    { ah => ah.Contract, ah => ah.User});
            return (list.Count() > 0) ? list : null;
        }
        public async Task<IList<ActionHistory>?> GetCommentActionByContractAnnexId(int contractAnnexId)
        {
            var list = await _genericRepository.WhereAsync(ah => ah.ContractAnnexId.Equals(contractAnnexId) &&
                                      ah.ActionType.Equals(ActionType.Commented), new System.Linq.Expressions.Expression<Func<ActionHistory, object>>[] { ah => ah.User,
                                                                                       ah => ah.Contract, ah => ah.User});
            return (list.Count() > 0) ? list : null;
        }

        //Get Create Action By ContractAnnexId
        public async Task<ActionHistory?> GetCreateActionByContractAnnexId(int contractAnnexId)
        {
            return await _genericRepository.FirstOrDefaultAsync(ah => ah.ContractAnnexId.Equals(contractAnnexId) &&
                                      ah.ActionType.Equals(ActionType.Created) && ah.ContractAnnexId != null, new System.Linq.Expressions.Expression<Func<ActionHistory, object>>[] { ah => ah.User,
                                                                                       ah => ah.Contract, ah => ah.User});
        }
        //get create contract annex action by userId
        public async Task<IList<ActionHistory>?> GetContractAnnexCreateActionByUserId(int userId)
        {
            var list = await _genericRepository.WhereAsync(ah => ah.UserId.Equals(userId) &&
                           ah.ActionType.Equals(ActionType.Created) && ah.ContractAnnexId != null, new System.Linq.Expressions.Expression<Func<ActionHistory, object>>[] { ah => ah.User,
                                                  ah => ah.ContractAnnex });
            return (list.Count() > 0) ? list : null;
        }

        public async Task<IList<ActionHistory>?> GetCreateActions() { 
            var list = await _genericRepository.WhereAsync(ah =>  ah.ContractId != null && ah.ActionType == ActionType.Created
            , new System.Linq.Expressions.Expression<Func<ActionHistory, object>>[] { ah => ah.User, ah => ah.Contract });
            return (list.Count() > 0) ? list : null;
        }
    }

}

