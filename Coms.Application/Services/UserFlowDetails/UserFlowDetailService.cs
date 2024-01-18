using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;

namespace Coms.Application.Services.UserFlowDetails
{
    public class UserFlowDetailService : IUserFlowDetailService
    {
        private readonly IContractFlowDetailsRepository _userFlowDetailsRepository;
        private readonly IFlowDetailRepository _flowDetailRepository;
        private readonly IActionHistoryRepository _actionHistoryRepository;
        private readonly IPartnerReviewRepository _partnerReviewRepository;

        public UserFlowDetailService(IContractFlowDetailsRepository userFlowDetailsRepository,
                IFlowDetailRepository flowDetailRepository,
                IActionHistoryRepository actionHistoryRepository,
                IPartnerReviewRepository partnerReviewRepository)
        {
            _userFlowDetailsRepository = userFlowDetailsRepository;
            _flowDetailRepository = flowDetailRepository;
            _actionHistoryRepository = actionHistoryRepository;
            _partnerReviewRepository = partnerReviewRepository;
        }

        public async Task<ErrorOr<PagingResult<UserFlowDetailResult>>> GetContractFlowDetails(int contractId,
                int currentPage, int pageSize)
        {
            var contractFlowDetails = await _userFlowDetailsRepository.GetByContractId(contractId);
            if (contractFlowDetails is not null)
            {
                IList<UserFlowDetailResult> results = new List<UserFlowDetailResult>();
                foreach (var contractFlowDetail in contractFlowDetails)
                {
                    var flowDetailResult = new UserFlowDetailResult()
                    {
                        Id = contractFlowDetail.Id,
                        Status = (int)contractFlowDetail.Status,
                        StatusString = contractFlowDetail.Status.ToString(),
                        ContractId = contractFlowDetail.ContractId,
                        UserId = (int)contractFlowDetail.FlowDetail.UserId,
                        FlowDetailId = contractFlowDetail.FlowDetailId,
                        FlowRole = contractFlowDetail.FlowDetail.FlowRole.ToString(),
                    };
                    var flowDetail = await _flowDetailRepository.GetFlowDetail(contractFlowDetail.FlowDetailId);
                    flowDetailResult.FullName = flowDetail.User.FullName;
                    if (flowDetail.User.Image is not null)
                    {
                        flowDetailResult.UserImage = flowDetail.User.Image;
                    }
                    results.Add(flowDetailResult);
                }
                int total = results.Count();
                if (currentPage > 0 && pageSize > 0)
                {
                    results = results.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                }
                return new PagingResult<UserFlowDetailResult>(results, total, currentPage, pageSize);
            }
            else
            {
                return Error.NotFound("404", "Not found any flow details!");
            }
        }

        //get contract annex flow details
        public async Task<ErrorOr<PagingResult<UserFlowDetailResult>>> GetContractAnnexFlowDetails(int contractAnnexId,
                           int currentPage, int pageSize)
        {
            var contractFlowDetails = await _userFlowDetailsRepository.GetByContractAnnexId(contractAnnexId);
            if (contractFlowDetails is not null)
            {
                IList<UserFlowDetailResult> results = new List<UserFlowDetailResult>();
                foreach (var contractFlowDetail in contractFlowDetails)
                {
                    var flowDetailResult = new UserFlowDetailResult()
                    {
                        Id = contractFlowDetail.Id,
                        Status = (int)contractFlowDetail.Status,
                        StatusString = contractFlowDetail.Status.ToString(),
                        ContractAnnexId = contractFlowDetail.ContractAnnexId,
                        UserId = (int)contractFlowDetail.FlowDetail.UserId,
                        FlowDetailId = contractFlowDetail.FlowDetailId,
                        FlowRole = contractFlowDetail.FlowDetail.FlowRole.ToString(),
                    };
                    var flowDetail = await _flowDetailRepository.GetFlowDetail(contractFlowDetail.FlowDetailId);
                    flowDetailResult.FullName = flowDetail.User.FullName;
                    if (flowDetail.User.Image is not null)
                    {
                        flowDetailResult.UserImage = flowDetail.User.Image;
                    }
                    results.Add(flowDetailResult);
                }
                int total = results.Count();
                if (currentPage > 0 && pageSize > 0)
                {
                    results = results.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                }
                return new PagingResult<UserFlowDetailResult>(results, total, currentPage, pageSize);
            }
            else
            {
                return Error.NotFound("404", "Not found any flow details!");
            }
        }

        public async Task<ErrorOr<PagingResult<NotificationResult>>> GetNotifications(int userId, int currentPage, int pageSize)
        {
            IList<NotificationResult> results = new List<NotificationResult>();
            var createActions = await _actionHistoryRepository.GetCreateActionByUserId(userId);
            if (createActions is not null)
            {
                
                foreach (var action in createActions)
                {
                    if (action.Contract.Status.Equals(DocumentStatus.Deleted))
                    {
                        continue;
                    }
                    else
                    {
                        var partnerReview = await _partnerReviewRepository.GetByContractId((int)action.ContractId);
                        if (partnerReview.IsApproved)
                        {
                            var notificationResult = new NotificationResult()
                            {
                                Title = "Partner Approved!",
                                Message = partnerReview.Partner.CompanyName + " approved your contract.",
                                Time = partnerReview.Contract.CreatedDate,
                                Long = AsTimeAgo(partnerReview.Contract.CreatedDate),
                                ContractId = partnerReview.ContractId,
                                Type = "Partner Review"
                            };
                            results.Add(notificationResult);
                        }
                        else
                        {
                            if (partnerReview.Status.Equals(PartnerReviewStatus.Inactive))
                            {
                                var notificationResult = new NotificationResult()
                                {
                                    Title = "Partner Rejected!",
                                    Message = partnerReview.Partner.CompanyName + " rejected your contract.",
                                    Time = partnerReview.Contract.CreatedDate,
                                    Long = AsTimeAgo(partnerReview.Contract.CreatedDate),
                                    ContractId = partnerReview.ContractId,
                                    Type = "Partner Review"
                                };
                                results.Add(notificationResult);
                            }
                        }
                    }
                }
            }
            var flowDetails = await _flowDetailRepository.GetUserFlowDetailsByUserId(userId);
            if (flowDetails is not null)
            {
                foreach (var flowDetail in flowDetails)
                {
                    var contractFlowDetails = await _userFlowDetailsRepository.GetByFlowDetailId(flowDetail.Id);
                    if(contractFlowDetails is not null)
                    {
                        foreach (var contractFlowDetail in contractFlowDetails)
                        {
                            if (contractFlowDetail.Contract.Status.Equals(DocumentStatus.Deleted))
                            {
                                continue;
                            }
                            else
                            {
                                var notificationResult = new NotificationResult()
                                {
                                    Title = "New Contract",
                                    Message = "You have a new contract to ",
                                    Time = contractFlowDetail.Contract.CreatedDate,
                                    Long = AsTimeAgo(contractFlowDetail.Contract.CreatedDate),
                                    ContractId = contractFlowDetail.ContractId,
                                    Type = "Approve"
                                };
                                if (flowDetail.FlowRole.Equals(FlowRole.Approver))
                                {
                                    notificationResult.Message += "approve!";
                                }
                                else
                                {
                                    notificationResult.Message += "sign!";
                                }
                                results.Add(notificationResult);
                            }
                        }
                    }
                    var contractAnnexFlowDetails = await _userFlowDetailsRepository.GetContractAnnexByFlowDetailId(flowDetail.Id);
                    if (contractAnnexFlowDetails is not null)
                    {
                        foreach (var contractAnnexFlowDetail in contractAnnexFlowDetails)
                        {
                            if (contractAnnexFlowDetail.Status.Equals(DocumentStatus.Deleted))
                            {
                                continue;
                            }
                            else
                            {
                                var notificationResult = new NotificationResult()
                                {
                                    Title = "New Contract Annex",
                                    Message = "You have a new contract annex to ",
                                    Time = contractAnnexFlowDetail.ContractAnnex.CreatedDate,
                                    Long = AsTimeAgo(contractAnnexFlowDetail.ContractAnnex.CreatedDate),
                                    ContractAnnexId = contractAnnexFlowDetail.ContractAnnexId,
                                    Type = "Approve"
                                };
                                if (flowDetail.FlowRole.Equals(FlowRole.Approver))
                                {
                                    notificationResult.Message += "approve!";
                                }
                                else
                                {
                                    notificationResult.Message += "sign!";
                                }
                                results.Add(notificationResult);
                            }
                        }
                    }
                }
            }
            if(results.Count() > 0)
            {
                int total = results.Count();
                results = results.OrderByDescending(nr => nr.Time).ToList();
                if (currentPage > 0 && pageSize > 0)
                {
                    results = results.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                }
                return new PagingResult<NotificationResult>(results, total, currentPage, pageSize);
            }
            else
            {
                return new PagingResult<NotificationResult>(new List<NotificationResult>(), 0, currentPage, pageSize);
            }
        }

        private string AsTimeAgo(DateTime dateTime)
        {
            TimeSpan timeSpan = DateTime.Now.Subtract(dateTime);

            return timeSpan.TotalSeconds switch
            {
                <= 60 => $"{timeSpan.Seconds} seconds ago",

                _ => timeSpan.TotalMinutes switch
                {
                    <= 1 => "About a minute ago",
                    < 60 => $"About {timeSpan.Minutes} minutes ago",
                    _ => timeSpan.TotalHours switch
                    {
                        <= 1 => "About an hour ago",
                        < 24 => $"About {timeSpan.Hours} hours ago",
                        _ => timeSpan.TotalDays switch
                        {
                            <= 1 => "yesterday",
                            <= 30 => $"About {timeSpan.Days} days ago",

                            <= 60 => "About a month ago",
                            < 365 => $"About {timeSpan.Days / 30} months ago",

                            <= 365 * 2 => "About a year ago",
                            _ => $"About {timeSpan.Days / 365} years ago"
                        }
                    }
                }
            };
        }
        public async Task<ErrorOr<UserFlowDetailResult>> AddContractFlowDetail(int status, int flowDetailId, int contractId, int liquidationRecordId, int contractAnnexId)
        {
            try
            {
                var contractFlowDetail = new Contract_FlowDetail
                {
                    Status = (FlowDetailStatus)status,
                    FlowDetailId = flowDetailId,
                    ContractId = contractId,
                    LiquidationRecordId = liquidationRecordId,
                    ContractAnnexId = contractAnnexId

                };
                await _userFlowDetailsRepository.AddContractFlowDetail(contractFlowDetail);
                var flowDetailResult = new UserFlowDetailResult()
                {
                    Id = contractFlowDetail.Id,
                    Status = (int)contractFlowDetail.Status,
                    StatusString = contractFlowDetail.Status.ToString(),
                    ContractId = contractFlowDetail.ContractId,
                    UserId = (int)contractFlowDetail.FlowDetail.UserId,
                    FlowDetailId = contractFlowDetail.FlowDetailId,
                    FlowRole = contractFlowDetail.FlowDetail.FlowRole.ToString(),
                };
                return flowDetailResult;

            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }
    }
}
