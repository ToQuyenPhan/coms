﻿using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.ActionHistories;
using Coms.Application.Services.Common;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
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
