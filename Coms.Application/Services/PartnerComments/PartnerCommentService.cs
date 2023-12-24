﻿using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
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

        public async Task<ErrorOr<PagingResult<PartnerCommentResult>>> GetPartnerComments(int contractId, int currentPage,
                int pageSize)
        {
            var partnerReview = await _partnerReviewRepository.GetByContractId(contractId);
            if (partnerReview is not null)
            {
                var partnerComments = await _partnerCommentRepository.GetByPartnerReviewId(partnerReview.Id);
                if (partnerComments is not null)
                {
                    partnerComments = partnerComments.OrderByDescending(pc => pc.CreatedAt).ToList();
                    IList<PartnerCommentResult> commentResults = new List<PartnerCommentResult>();
                    foreach (var partnerComment in partnerComments)
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
                        commentResults.Add(commentResult);
                    }
                    int total = commentResults.Count();
                    if (currentPage > 0 && pageSize > 0)
                    {
                        commentResults = commentResults.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                    }
                    return new PagingResult<PartnerCommentResult>(commentResults, total, currentPage, pageSize);
                }
                else
                {
                    return new PagingResult<PartnerCommentResult>(new List<PartnerCommentResult>(), 0, currentPage, pageSize);
                }
            }
            else
            {
                return Error.NotFound("404", "Contract not found!");
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