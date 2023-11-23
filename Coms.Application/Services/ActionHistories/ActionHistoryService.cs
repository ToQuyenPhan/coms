using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Accesses;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.ActionHistories
{
    public class ActionHistoryService : IActionHistoryService
    {
        private readonly IActionHistoryRepository _actionHistoryRepository;

        public ActionHistoryService(IActionHistoryRepository actionHistoryRepository)
        {
            _actionHistoryRepository = actionHistoryRepository;
        }
        public async Task<ErrorOr<ActionHistoryResult>> AddActionHistory(int userId, int contractId, int actionType)
        {
            try
            {
                var actionHistory = new ActionHistory
                {
                     ActionType = (ActionType)actionType,
                     UserId = userId,
                     CreatedAt = DateTime.Now,
                     ContractId = contractId, 
                };
                await _actionHistoryRepository.AddActionHistory(actionHistory);
                var created = _actionHistoryRepository.GetActionHistoryById(actionHistory.Id).Result;
                var IActionHistoryResult = new ActionHistoryResult
                { 
                   Id = actionHistory.Id,
                   ActionType = actionHistory.ActionType.ToString(),
                   ContractId=actionHistory.ContractId,
                    ContractName = created.Contract.ContractName,
                    CreatedAt = actionHistory.CreatedAt,
                   UserId =actionHistory.UserId,
                    UserName = created.User.FullName
                };
                return IActionHistoryResult;

            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }
    }
}
