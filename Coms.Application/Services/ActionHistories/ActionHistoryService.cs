using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;
using OfficeOpenXml;
using OfficeOpenXml.Style;

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
                            if (actionHistory.Contract.Status.Equals(DocumentStatus.Deleted))
                            {
                                continue;
                            }
                            else
                            {
                                if (!actionHistories.Contains(actionHistory))
                                {
                                    actionHistories.Add(actionHistory);
                                }
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
                    actionHistoryResults.Add(actionHistoryResult);
                }
                actionHistoryResults = actionHistoryResults.OrderByDescending(ah => ah.CreatedAt).ToList();
                if (currentPage > 0 && pageSize > 0)
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

        public async Task<ErrorOr<MemoryStream>> ExportActionHistories(int userId)
        {
            try
            {
                var createHistories = await _actionHistoryRepository.GetCreateActionByUserId(userId);
                if (createHistories is not null)
                {
                    IList<ActionHistory> actionHistories = new List<ActionHistory>();
                    foreach (var history in createHistories)
                    {
                        var actionHistoryList = await _actionHistoryRepository
                                .GetOtherUserActionByContractId(history.ContractId, userId);
                        if (actionHistoryList is not null)
                        {
                            foreach (var actionHistory in actionHistoryList)
                            {
                                if (actionHistory.Contract.Status.Equals(DocumentStatus.Deleted))
                                {
                                    continue;
                                }
                                else
                                {
                                    if (!actionHistories.Contains(actionHistory))
                                    {
                                        actionHistories.Add(actionHistory);
                                    }
                                }
                            }
                        }
                    }
                    ExcelPackage excel = new ExcelPackage();
                    var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Row(1).Height = 20;
                    workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(1).Style.Font.Bold = true;
                    workSheet.Cells[1, 1].Value = "S.No";
                    workSheet.Cells[1, 2].Value = "Full Name";
                    workSheet.Cells[1, 3].Value = "Action Type";
                    workSheet.Cells[1, 4].Value = "Contract Name";
                    workSheet.Cells[1, 5].Value = "Created At";
                    int recordIndex = 2;
                    foreach (var actionHistory in actionHistories)
                    {
                        workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                        workSheet.Cells[recordIndex, 2].Value = actionHistory.User.FullName;
                        workSheet.Cells[recordIndex, 3].Value = actionHistory.ActionType.ToString();
                        workSheet.Cells[recordIndex, 4].Value = actionHistory.Contract.ContractName;
                        workSheet.Cells[recordIndex, 5].Value = actionHistory.CreatedAt.ToString("dd/MM/yyyy hh:mm:ss");
                        recordIndex++;
                    }
                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).AutoFit();
                    workSheet.Column(4).AutoFit();
                    workSheet.Column(5).AutoFit();
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    memoryStream.Position = 0;
                    return memoryStream;
                }
                else
                {
                    return Error.Conflict("409", "Not found any actions to exports!");
                }
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
                //actionHistories = _actionHistoryRepository.GetCreateActionByContractId(contractId).Result;
                var results = new List<ActionHistoryResult>();
                //if (actionHistories != null)
                //{
                //    foreach (var actionHistory in actionHistories)
                //    {
                //        var result = new ActionHistoryResult()
                //        {
                //            Id = actionHistory.Id,
                //            ActionType = (int)actionHistory.ActionType,
                //            ActionTypeString = actionHistory.ActionType.ToString(),
                //            CreatedAt = actionHistory.CreatedAt,
                //            CreatedAtString = actionHistory.CreatedAt.ToString("dd/MM/yyyy"),
                //            UserId = actionHistory.UserId,
                //            FullName = actionHistory.User.FullName,
                //            UserImage = actionHistory.User.Image,
                //            ContractId = actionHistory.ContractId,
                //            ContractName = actionHistory.Contract.ContractName
                //        };
                //        results.Add(result);
                //    }
                //}
                return results;
            }
            catch (Exception ex)
            {
                return Error.NotFound("Action histories not found!");
            }
        }
    }
}
