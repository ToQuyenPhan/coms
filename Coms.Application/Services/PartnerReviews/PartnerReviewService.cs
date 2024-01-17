using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;

namespace Coms.Application.Services.PartnerReviews
{
    public class PartnerReviewService : IPartnerReviewService
    {
        private readonly IPartnerReviewRepository _partnerReviewRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPartnerRepository _partnerRepository;
        private readonly IActionHistoryRepository _actionHistoryRepository;
        private readonly IContractRepository _contractRepository;

        public PartnerReviewService(IPartnerReviewRepository partnerReviewRepository,
            IUserRepository userRepository,
            IContractRepository contractRepository,
            IPartnerRepository partnerRepository,
            IActionHistoryRepository actionHistoryRepository)
        {
            _partnerReviewRepository = partnerReviewRepository;
            _userRepository = userRepository;
            _partnerRepository = partnerRepository;
            _actionHistoryRepository = actionHistoryRepository;
            _contractRepository = contractRepository;
        }

        public async Task<ErrorOr<PartnerReviewResult>> AddPartnerReview(int partnerId, int userId, int contractId)
        {
            try
            {
                var user = _userRepository.GetUser(userId).Result;
                //var contract =  _contractRepository.GetContract(contractId).Result;
                var partner = _partnerRepository.GetPartner(partnerId).Result;
                var partnerReview = new PartnerReview
                {
                    ContractId = contractId,
                    PartnerId = partnerId,
                    UserId = userId,
                    IsApproved = false,
                    SendDate = DateTime.Now,
                    ReviewAt = DateTime.Now,
                    Status = PartnerReviewStatus.Active
                };
                await _partnerReviewRepository.AddPartnerReview(partnerReview);
                var createdPartnerReview = await _partnerReviewRepository.GetPartnerReview(partnerReview.Id);
                var partnerReviewResult = new PartnerReviewResult
                {
                    Id = createdPartnerReview.Id,
                    ContractId = createdPartnerReview.ContractId,
                    ContractName = createdPartnerReview.Contract.ContractName,
                    IsApproved = false,
                    PartnerId = createdPartnerReview.Id,
                    PartnerCompanyName = createdPartnerReview.Partner.CompanyName,
                    ReviewAt = DateTime.Now,
                    SendDate = DateTime.Now,
                    UserId = createdPartnerReview.UserId,
                    UserName = createdPartnerReview.User.Username
                };
                return partnerReviewResult;
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<PartnerReviewResult>> ApprovePartnerReview(int contractId, bool isApproved)
        {
            try
            {
                var partnerPreview = await _partnerReviewRepository.GetByContractId(contractId);
                if (partnerPreview is not null)
                {
                    if (isApproved)
                    {
                        partnerPreview.IsApproved = true;
                    }
                    else
                    {
                        partnerPreview.Status = PartnerReviewStatus.Inactive;
                    }
                    partnerPreview.ReviewAt = DateTime.Now;
                    await _partnerReviewRepository.UpdatePartnerPreview(partnerPreview);
                    var partnerReviewResult = new PartnerReviewResult
                    {
                        Id = partnerPreview.Id,
                        ContractId = partnerPreview.ContractId,
                        ContractName = partnerPreview.Contract.ContractName,
                        IsApproved = partnerPreview.IsApproved,
                        PartnerId = partnerPreview.Id,
                        PartnerCompanyName = partnerPreview.Partner.CompanyName,
                        UserId = partnerPreview.UserId,
                        UserName = partnerPreview.User.Username
                    };
                    return partnerReviewResult;
                }
                else
                {
                    return Error.NotFound("404", "Partner Review is not found!");
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<PagingResult<NotificationResult>>> GetNotifications(int userId, int currentPage, int pageSize)
        {
            try
            {
                var createActions = await _actionHistoryRepository.GetCreateActionByUserId(userId);
                if (createActions is not null)
                {
                    IList<NotificationResult> results = new List<NotificationResult>();
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
                                    Time = partnerReview.ReviewAt,
                                    ContractId = partnerReview.ContractId
                                };
                                if(partnerReview.ReviewAt is not null)
                                {
                                    notificationResult.Long = AsTimeAgo((DateTime)partnerReview.ReviewAt);
                                }
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
                                        Time = partnerReview.ReviewAt,
                                        ContractId = partnerReview.ContractId
                                    };
                                    if (partnerReview.ReviewAt is not null)
                                    {
                                        notificationResult.Long = AsTimeAgo((DateTime)partnerReview.ReviewAt);
                                    }
                                    results.Add(notificationResult);
                                }
                            }
                        }
                    }
                    results = results.OrderByDescending(nr => nr.Time).ToList();
                    int total = results.Count();
                    if (currentPage > 0 && pageSize > 0)
                    {
                        results = results.Skip((currentPage - 1) * pageSize).Take(pageSize)
                                .ToList();
                    }
                    return new PagingResult<NotificationResult>(results, total, currentPage, pageSize);
                }
                else
                {
                    return new PagingResult<NotificationResult>(new List<NotificationResult>(), 0, currentPage, pageSize);
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<PagingResult<NotificationResult>>> GetPartnerNotifications(int partnerId, int currentPage, int pageSize)
        {
            try
            {
                var partnerReviews = await _partnerReviewRepository.GetByPartnerId(partnerId, false);
                if (partnerReviews is not null)
                {
                    IList<NotificationResult> results = new List<NotificationResult>();
                    foreach (var partnerReview in partnerReviews)
                    {
                        var notificationResult = new NotificationResult()
                        {
                            Title = "New Contract!",
                            Message = "You have new contract to approved.",
                            Time = partnerReview.SendDate,
                            ContractId = partnerReview.ContractId
                        };
                        if(partnerReview.SendDate is not null)
                        {
                            notificationResult.Long = AsTimeAgo((DateTime)partnerReview.SendDate);
                        }
                        results.Add(notificationResult);
                    }
                    results = results.OrderByDescending(nr => nr.Time).ToList();
                    int total = results.Count();
                    if (currentPage > 0 && pageSize > 0)
                    {
                        results = results.Skip((currentPage - 1) * pageSize).Take(pageSize)
                                .ToList();
                    }
                    return new PagingResult<NotificationResult>(results, total, currentPage, pageSize);
                }
                else
                {
                    return new PagingResult<NotificationResult>(new List<NotificationResult>(), 0, currentPage, pageSize);
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
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
