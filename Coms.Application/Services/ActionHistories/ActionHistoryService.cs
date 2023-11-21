using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Domain.Entities;
using ErrorOr;

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
                    else
                    {
                        return new PagingResult<ActionHistoryResult>(new List<ActionHistoryResult>(), 0, currentPage,
                            pageSize);
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
    }
}
