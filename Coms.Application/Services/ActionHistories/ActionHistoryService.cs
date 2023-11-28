using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
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

        public async Task<ErrorOr<PagingResult<ActionHistoryResult>>> GetRecentActivities(int userId, int currentPage,
                int pageSize)
        {
            if (_actionHistoryRepository.GetCreateActionByUserId(userId).Result is not null)
            {
                var createHistories = await _actionHistoryRepository.GetCreateActionByUserId(userId);
                IList<ActionHistory> actionHistories = new List<ActionHistory>();
                foreach (var history in createHistories)
                {
                    var actionHistoryList = await _actionHistoryRepository
                            .GetOtherUserActionByContractId(history.ContractId, userId);
                    if (actionHistoryList is not null)
                    {
                        foreach (var actionHistory in actionHistoryList)
                        {
                            if (!actionHistories.Contains(actionHistory))
                            {
                                actionHistories.Add(actionHistory);
                            }
                        }
                    }
                }
                IList<ActionHistoryResult> actionHistoryResults = new List<ActionHistoryResult>();
                foreach (var actionHistory in actionHistories)
                {
                    var actionHistoryResult = new ActionHistoryResult()
                    {
                        Id = actionHistory.Id,
                        ActionType = (int) actionHistory.ActionType,
                        ActionTypeString = actionHistory.ActionType.ToString(),
                        CreatedAt = actionHistory.CreatedAt,
                        CreatedAtString = actionHistory.CreatedAt.ToString("dd/MM/yyyy"),
                        UserId = actionHistory.UserId,
                        FullName = actionHistory.User.FullName,
                        UserImage = actionHistory.User.Image,
                        ContractId = actionHistory.ContractId,
                        ContractName = actionHistory.Contract.ContractName
                    };
                    actionHistoryResults.Add(actionHistoryResult);
                }
                if(currentPage > 0 && pageSize > 0)
                {
                    actionHistoryResults = actionHistoryResults.Skip((currentPage - 1) * pageSize).Take(pageSize)
                            .ToList();
                }
                return new PagingResult<ActionHistoryResult>(actionHistoryResults, actionHistories.Count(), currentPage, pageSize);
            }
            else
            {
                return new PagingResult<ActionHistoryResult>(new List<ActionHistoryResult>(), 0, currentPage,
                    pageSize);
            }
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
                    Id = created.Id,
                    ActionType = (int)created.ActionType,
                    ContractId = created.ContractId,
                    ContractName = created.Contract.ContractName,
                    CreatedAt = created.CreatedAt,
                    CreatedAtString = created.CreatedAt.ToString(),
                    UserId = created.UserId,
                    FullName = created.User.FullName,
                    UserImage = created.User.Image,
                    ActionTypeString = created.ActionType.ToString(),
                };
                return IActionHistoryResult;

            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        //add get action history by contract id
        public async Task<ErrorOr<IList<ActionHistoryResult>>> GetActionHistoryByContractId(int contractId)
        {
            try
            {
                IList<ActionHistory> actionHistories = new List<ActionHistory>();
                actionHistories = _actionHistoryRepository.GetCreateActionByContractId(contractId).Result;
                var results = new List<ActionHistoryResult>();
                if (actionHistories != null)
                {
                    foreach (var actionHistory in actionHistories)
                    {
                        var result = new ActionHistoryResult()
                        {
                            Id = actionHistory.Id,
                            ActionType = (int)actionHistory.ActionType,
                            ActionTypeString = actionHistory.ActionType.ToString(),
                            CreatedAt = actionHistory.CreatedAt,
                            CreatedAtString = actionHistory.CreatedAt.ToString("dd/MM/yyyy"),
                            UserId = actionHistory.UserId,
                            FullName = actionHistory.User.FullName,
                            UserImage = actionHistory.User.Image,
                            ContractId = actionHistory.ContractId,
                            ContractName = actionHistory.Contract.ContractName
                        };
                        results.Add(result);
                    }
                }
                return results;
            }
            catch (Exception ex)
            {
                return Error.NotFound("Action histories not found!");
            }
        }
    }
}
