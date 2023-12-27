﻿using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;

namespace Coms.Application.Services.Comments
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IActionHistoryRepository _actionHistoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IContractFlowDetailsRepository _userFlowDetailsRepository;
        private readonly IFlowDetailRepository _flowDetailRepository;

        public CommentService(ICommentRepository commentRepository,
            IActionHistoryRepository actionHistoryRepository,
            IUserRepository userRepository,
            IContractRepository contractRepository,
            IContractFlowDetailsRepository userFlowDetailsRepository,
            IFlowDetailRepository flowDetailRepository)
        {
            _commentRepository = commentRepository;
            _actionHistoryRepository = actionHistoryRepository;
            _userRepository = userRepository;
            _contractRepository = contractRepository;
            _userFlowDetailsRepository = userFlowDetailsRepository;
            _flowDetailRepository = flowDetailRepository;
        }

        public async Task<ErrorOr<PagingResult<CommentResult>>> GetAllComments(int userId, int currentPage,
                int pageSize)
        {
            IList<ActionHistory>? createActions = await _actionHistoryRepository.GetCreateActionByUserId(userId);
            if (createActions is not null)
            {
                IList<ActionHistory> commentHistories = new List<ActionHistory>();
                foreach (ActionHistory action in createActions)
                {
                    IList<ActionHistory>? commentHistoryList = await _actionHistoryRepository
                            .GetCommentActionByContractId(action.ContractId, userId);
                    if (commentHistoryList is not null)
                    {
                        foreach (ActionHistory commentHistory in commentHistoryList)
                        {
                            if (!commentHistories.Contains(commentHistory))
                            {
                                commentHistories.Add(commentHistory);
                            }
                        }
                    }
                }
                if (commentHistories.Count() > 0)
                {
                    IList<CommentResult> comments = new List<CommentResult>();
                    foreach (ActionHistory commentHistory in commentHistories)
                    {
                        Comment? comment = await _commentRepository.GetByActionHistoryId(commentHistory.Id);
                        if (comment is not null)
                        {
                            CommentResult commentResult = new()
                            {
                                Id = comment.Id,
                                Content = comment.Content,
                                ActionHistoryId = comment.ActionHistoryId,
                                ReplyId = comment.ReplyId,
                                Status = (int)comment.Status,
                                StatusString = comment.Status.ToString(),
                                Long = AsTimeAgo(comment.ActionHistory.CreatedAt),
                                CreatedAt = comment.ActionHistory.CreatedAt.ToString()
                            };
                            User? user = await _userRepository.GetUser((int)comment.ActionHistory.UserId);
                            commentResult.UserId = user.Id;
                            commentResult.FullName = user.FullName;
                            comments.Add(commentResult);
                        }
                    }
                    int total = comments.Count();
                    if (currentPage > 0 && pageSize > 0)
                    {
                        comments = comments.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                    }
                    return new PagingResult<CommentResult>(comments, total, currentPage, pageSize);
                }
                else
                {
                    return new PagingResult<CommentResult>(new List<CommentResult>(), 0, currentPage,
                    pageSize);
                }
            }
            else
            {
                return new PagingResult<CommentResult>(new List<CommentResult>(), 0, currentPage,
                    pageSize);
            }
        }

        public async Task<ErrorOr<CommentResult>> DismissComment(int id)
        {
            try
            {
                Comment? comment = await _commentRepository.GetComment(id);
                if (comment is not null)
                {
                    comment.Status = Domain.Enum.CommentStatus.Dismissed;
                    await _commentRepository.UpdateComment(comment);
                    CommentResult commentResult = new()
                    {
                        Id = comment.Id,
                        Content = comment.Content,
                        ActionHistoryId = comment.ActionHistoryId,
                        ReplyId = comment.ReplyId,
                        Status = (int)comment.Status,
                        StatusString = comment.Status.ToString()
                    };
                    return commentResult;
                }
                else
                {
                    return Error.NotFound("404", "Comment is not found!");
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<PagingResult<CommentResult>>> GetContractComments(int contractId, int currentPage,
                           int pageSize)
        {
            IList<ActionHistory>? histories = await _actionHistoryRepository.GetCommentActionByContractId(contractId);
            if (histories is not null)
            {
                IList<CommentResult> comments = new List<CommentResult>();
                foreach (ActionHistory commentHistory in histories)
                {
                    Comment? comment = await _commentRepository.GetByActionHistoryId(commentHistory.Id);
                    if (comment is not null)
                    {
                        CommentResult commentResult = new()
                        {
                            Id = comment.Id,
                            Content = comment.Content,
                            ActionHistoryId = comment.ActionHistoryId,
                            ReplyId = comment.ReplyId,
                            Status = (int)comment.Status,
                            StatusString = comment.Status.ToString(),
                            Long = AsTimeAgo(commentHistory.CreatedAt),
                            CreatedAt = commentHistory.CreatedAt.ToString(),
                            UserId = commentHistory.User.Id,
                            FullName = commentHistory.User.FullName
                        };
                        if (commentHistory.User.Image is not null)
                        {
                            commentResult.UserImage = commentHistory.User.Image;
                        }
                        IList<Contract_FlowDetail>? userFlowDetails =
                            await _userFlowDetailsRepository.GetByContractId(commentHistory.ContractId);
                        if (userFlowDetails is not null)
                        {
                            int numberOfRole = 0;
                            foreach (Contract_FlowDetail userFlowDetail in userFlowDetails)
                            {
                                FlowDetail? flowDetail = await _flowDetailRepository.GetFlowDetail(userFlowDetail.FlowDetailId);
                                if (flowDetail is not null)
                                {
                                    numberOfRole++;
                                    if (numberOfRole > 1)
                                    {
                                        commentResult.AccessRole += ", " + flowDetail.FlowRole.ToString();
                                    }
                                    else
                                    {
                                        commentResult.AccessRole = flowDetail.FlowRole.ToString();
                                    }
                                }
                            }
                        }
                        else
                        {
                            commentResult.AccessRole = "Author";
                        }
                        comments.Add(commentResult);
                    }
                }
                int total = comments.Count();
                comments = comments.OrderByDescending(c => c.CreatedAt).ToList();
                if (currentPage > 0 && pageSize > 0)
                {
                    comments = comments.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                }
                return new PagingResult<CommentResult>(comments, total, currentPage, pageSize);
            }
            else
            {
                return new PagingResult<CommentResult>(new List<CommentResult>(), 0, currentPage,
                                       pageSize);
            }
        }

        public async Task<ErrorOr<CommentDetailResult>> GetCommentDetail(int id)
        {
            Comment? comment = await _commentRepository.GetComment(id);
            if (comment is not null)
            {
                if (comment.Status.Equals(CommentStatus.Inactive))
                {
                    return Error.Conflict("409", "Comment is inactive!");
                }
                Contract? contract = await _contractRepository.GetContract(comment.ActionHistory.ContractId);
                if (contract is not null)
                {
                    if (contract.Status.Equals(DocumentStatus.Deleted))
                    {
                        return Error.Conflict("409", "Contract no longer exists!");
                    }
                    else
                    {
                        CommentDetailResult result = new()
                        {
                            ContractId = contract.Id
                        };
                        return result;
                    }
                }
                else
                {
                    return Error.NotFound("404", "Contract is not found!");
                }
            }
            else
            {
                return Error.NotFound("404", "Comment is not found!");
            }
        }

        public async Task<ErrorOr<CommentResult>> LeaveComment(int userId, int contractId, string content,
        int? replyId)
        {
            try
            {
                Contract? contract = await _contractRepository.GetContract(contractId);
                if (contract is not null)
                {
                    if (contract.Status.Equals(DocumentStatus.Deleted))
                    {
                        return Error.Conflict("409", "Contract no longer exist!");
                    }
                    ActionHistory actionHistory = new()
                    {
                        ActionType = ActionType.Commented,
                        CreatedAt = DateTime.Now,
                        UserId = userId,
                        ContractId = contractId
                    };
                    await _actionHistoryRepository.AddActionHistory(actionHistory);
                    Comment comment = new()
                    {
                        Content = content,
                        Status = CommentStatus.Active,
                        ActionHistoryId = actionHistory.Id
                    };
                    if (replyId is not null and > 0)
                    {
                        comment.ReplyId = replyId;
                    }
                    ActionHistory actionHistorycreated = await _actionHistoryRepository.GetActionHistoryById(actionHistory.Id);
                    await _commentRepository.AddComment(comment);
                    CommentResult commentResult = new()
                    {
                        Id = comment.Id,
                        Content = comment.Content,
                        ActionHistoryId = comment.ActionHistoryId,
                        ReplyId = comment.ReplyId,
                        Status = (int)comment.Status,
                        StatusString = comment.Status.ToString(),
                        UserId = (int)actionHistorycreated.UserId,
                        FullName = actionHistorycreated.User.FullName,
                        UserImage = actionHistorycreated.User.Image,
                        CreatedAt = actionHistorycreated.CreatedAt.ToString(),
                        Long = AsTimeAgo(actionHistorycreated.CreatedAt)
                    };
                    return commentResult;
                }
                else
                {
                    return Error.NotFound("404", "Contract is not found!");
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<CommentResult>> DeleteComment(int id)
        {
            try
            {
                Comment? comment = await _commentRepository.GetComment(id);
                if (comment is not null)
                {
                    comment.Status = CommentStatus.Inactive;
                    await _commentRepository.UpdateComment(comment);
                    CommentResult commentResult = new()
                    {
                        Id = comment.Id,
                        Content = comment.Content,
                        ActionHistoryId = comment.ActionHistoryId,
                        ReplyId = comment.ReplyId,
                        Status = (int)comment.Status,
                        StatusString = comment.Status.ToString()
                    };
                    return commentResult;
                }
                else
                {
                    return Error.NotFound("404", "Comment is not found!");
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<CommentResult>> EditComment(int id, string content)
        {
            try
            {
                Comment? comment = await _commentRepository.GetComment(id);
                if (comment is not null)
                {
                    //var actionHistory = await _actionHistoryRepository.GetActionHistoryById((int)comment.ActionHistoryId); 
                    //actionHistory.CreatedAt = DateTime.Now;
                    //await _actionHistoryRepository.UpdateActionHistory(actionHistory);
                    comment.Content = content;
                    await _commentRepository.UpdateComment(comment);
                    ActionHistory actionHistory = await _actionHistoryRepository.GetActionHistoryById(comment.ActionHistory.Id);
                    CommentResult commentResult = new()
                    {
                        Id = comment.Id,
                        Content = comment.Content,
                        ActionHistoryId = comment.ActionHistoryId,
                        ReplyId = comment.ReplyId,
                        Status = (int)comment.Status,
                        StatusString = comment.Status.ToString(),
                        CreatedAt = comment.ActionHistory.CreatedAt.ToString(),
                        FullName = actionHistory.User.FullName,
                        UserId = (int)actionHistory.UserId,
                        UserImage = actionHistory.User.Image,
                        Long = AsTimeAgo(actionHistory.CreatedAt),
                    };
                    return commentResult;
                }
                else
                {
                    return Error.NotFound("404", "Comment is not found!");
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
