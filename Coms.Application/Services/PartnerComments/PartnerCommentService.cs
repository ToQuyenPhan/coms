using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using ErrorOr;

namespace Coms.Application.Services.PartnerComments
{
    public class PartnerCommentService : IPartnerCommentService
    {
        private readonly IPartnerReviewRepository _partnerReviewRepository;
        private readonly IPartnerCommentRepository _partnerCommentRepository;

        public PartnerCommentService(IPartnerReviewRepository partnerReviewRepository,
                IPartnerCommentRepository partnerCommentRepository)
        {
            _partnerReviewRepository = partnerReviewRepository;
            _partnerCommentRepository = partnerCommentRepository;
        }

        public async Task<ErrorOr<PartnerCommentResult>> GetPartnerComment(int contractId)
        {
            var partnerReview = await _partnerReviewRepository.GetByContractId(contractId);
            if (partnerReview is not null)
            {
                var partnerComment = await _partnerCommentRepository.GetByPartnerReviewId(partnerReview.Id);
                if (partnerComment is not null)
                {
                    var commentResult = new PartnerCommentResult()
                    {
                        Id = partnerComment.Id,
                        Content = partnerComment.Content,
                        ReplyId = partnerComment.Id,
                        PartnerReviewId = partnerComment.PartnerReviewId,
                        CreatedAt = partnerComment.CreatedAt.ToString(),
                        Long = AsTimeAgo(partnerComment.CreatedAt)
                    };
                    return commentResult;
                }
                else
                {
                    return Error.NotFound("404", "Not have any comment");
                }
            }
            else
            {
                return Error.NotFound("404", "Contract not found!");
            }
        }

        //get partner comment by contract annex id
        public async Task<ErrorOr<PartnerCommentResult>> GetPartnerCommentByContractAnnexId(int contractAnnexId)
        {
            var partnerReview = await _partnerReviewRepository.GetByContractAnnexId(contractAnnexId);
            if (partnerReview is not null)
            {
                var partnerComment = await _partnerCommentRepository.GetByPartnerReviewId(partnerReview.Id);
                if (partnerComment is not null)
                {
                    var commentResult = new PartnerCommentResult()
                    {
                        Id = partnerComment.Id,
                        Content = partnerComment.Content,
                        ReplyId = partnerComment.Id,
                        PartnerReviewId = partnerComment.PartnerReviewId,
                        CreatedAt = partnerComment.CreatedAt.ToString(),
                        Long = AsTimeAgo(partnerComment.CreatedAt)
                    };
                    return commentResult;
                }
                else
                {
                    return Error.NotFound("404", "Not have any comment");
                }
            }
            else
            {
                return Error.NotFound("404", "Contract annex not found!");
            }
        }
        public async Task<ErrorOr<PartnerCommentResult>> AddPartnerComment(int contractId, string content)
        {
            try
            {
                var partnerReview = await _partnerReviewRepository.GetByContractId(contractId);
                if (partnerReview is not null)
                {
                    var partnerComment = new PartnerComment()
                    {
                        Content = content,
                        CreatedAt = DateTime.Now,
                        PartnerReviewId = partnerReview.Id,
                    };
                    await _partnerCommentRepository.AddPartnerComment(partnerComment);
                    var commentResult = new PartnerCommentResult()
                    {
                        Id = partnerComment.Id,
                        Content = partnerComment.Content,
                        ReplyId = partnerComment.Id,
                        PartnerReviewId = partnerComment.PartnerReviewId,
                        CreatedAt = partnerComment.CreatedAt.ToString(),
                        Long = AsTimeAgo(partnerComment.CreatedAt)
                    };
                    return commentResult;
                }
                else
                {
                    return Error.NotFound("404", "Partner review not found!");
                }
            }catch(Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<PartnerCommentResult>> AddAnnexPartnerComment(int contractAnnexId, string content)
        {
            try
            {
                var partnerReview = await _partnerReviewRepository.GetByContractAnnexId(contractAnnexId);
                if (partnerReview is not null)
                {
                    var partnerComment = new PartnerComment()
                    {
                        Content = content,
                        CreatedAt = DateTime.Now,
                        PartnerReviewId = partnerReview.Id,
                    };
                    await _partnerCommentRepository.AddPartnerComment(partnerComment);
                    var commentResult = new PartnerCommentResult()
                    {
                        Id = partnerComment.Id,
                        Content = partnerComment.Content,
                        ReplyId = partnerComment.Id,
                        PartnerReviewId = partnerComment.PartnerReviewId,
                        CreatedAt = partnerComment.CreatedAt.ToString(),
                        Long = AsTimeAgo(partnerComment.CreatedAt)
                    };
                    return commentResult;
                }
                else
                {
                    return Error.NotFound("404", "Partner review not found!");
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
