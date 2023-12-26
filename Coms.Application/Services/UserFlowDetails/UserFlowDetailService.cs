using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Domain.Enum;
using ErrorOr;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace Coms.Application.Services.UserFlowDetails
{
    public class UserFlowDetailService : IUserFlowDetailService
    {
        private readonly IContractFlowDetailsRepository _userFlowDetailsRepository;
        private readonly IFlowDetailRepository _flowDetailRepository;

        public UserFlowDetailService(IContractFlowDetailsRepository userFlowDetailsRepository,
                IFlowDetailRepository flowDetailRepository)
        {
            _userFlowDetailsRepository = userFlowDetailsRepository;
            _flowDetailRepository = flowDetailRepository;
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

        public async Task<ErrorOr<PagingResult<NotificationResult>>> GetNotifications(int userId, int currentPage, int pageSize)
        {
            var flowDetails = await _flowDetailRepository.GetUserFlowDetailsByUserId(userId);
            if (flowDetails is not null)
            {
                IList<NotificationResult> results = new List<NotificationResult>();
                foreach (var flowDetail in flowDetails)
                {
                    var contractFlowDetails = await _userFlowDetailsRepository.GetByFlowDetailId(flowDetail.Id);
                    if(contractFlowDetails is not null)
                    {
                        contractFlowDetails = contractFlowDetails.OrderByDescending(cfd => cfd.Contract.CreatedDate).ToList();
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
                                    Long = AsTimeAgo(contractFlowDetail.Contract.CreatedDate),
                                    ContractId = contractFlowDetail.ContractId,
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
                int total = results.Count();
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
    }
}
