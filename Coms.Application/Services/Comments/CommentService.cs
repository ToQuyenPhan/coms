using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Domain.Entities;
using ErrorOr;

namespace Coms.Application.Services.Comments
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IActionHistoryRepository _actionHistoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAccessRepository _accessRepository;
        private readonly IUserAccessRepository _userAccessRepository;

        public CommentService(ICommentRepository commentRepository,
            IActionHistoryRepository actionHistoryRepository,
            IUserRepository userRepository,
            IAccessRepository accessRepository,
            IUserAccessRepository userAccessRepository)
        {
            _commentRepository = commentRepository;
            _actionHistoryRepository = actionHistoryRepository;
            _userRepository = userRepository;
            _accessRepository = accessRepository;
            _userAccessRepository = userAccessRepository;
        }

        public async Task<ErrorOr<PagingResult<CommentResult>>> GetAllComments(int userId, int currentPage,
                int pageSize)
        {
            if (_actionHistoryRepository.GetCreateActionByUserId(userId).Result is not null)
            {
                var createHistories = await _actionHistoryRepository.GetCreateActionByUserId(userId);
                IList<ActionHistory> commentHistories = new List<ActionHistory>();
                foreach (var history in createHistories)
                {
                    var commentHistoryList = await _actionHistoryRepository
                            .GetCommentActionByContractId(history.ContractId, userId);
                    if (commentHistoryList is not null)
                    {
                        foreach (var commentHistory in commentHistoryList)
                        {
                            if (!commentHistories.Contains(commentHistory))
                            {
                                commentHistories.Add(commentHistory);
                            }
                        }
                    }
                }
                IList<CommentResult> comments = new List<CommentResult>();
                foreach (var commentHistory in commentHistories)
                {
                    var comment = await _commentRepository.GetByActionHistoryId(commentHistory.Id);
                    if(comment is not null)
                    {
                        var commentResult = new CommentResult()
                        {
                            Id = comment.Id,
                            Content = comment.Content,
                            ActionHistoryId = comment.ActionHistoryId,
                            ReplyId = comment.ReplyId,
                            Status = (int)comment.Status,
                            StatusString = comment.Status.ToString()
                        };
                        commentResult.Long = AsTimeAgo(comment.ActionHistory.CreatedAt);
                        commentResult.CreatedAt = comment.ActionHistory.CreatedAt.ToString();
                        var user = await _userRepository.GetUser((int)comment.ActionHistory.UserId);
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

        public async Task<ErrorOr<CommentResult>> DismissComment(int id) {
            try
            {
                if(_commentRepository.GetComment(id).Result is not null)
                {
                    var comment = await _commentRepository.GetComment(id);
                    comment.Status = Domain.Enum.CommentStatus.Dismissed;
                    await _commentRepository.UpdateComment(comment);
                    var commentResult = new CommentResult()
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
                    return Error.NotFound();
                }
            }catch(Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        //add get all comment of a contract
        public async Task<ErrorOr<PagingResult<CommentResult>>> GetContractComments(int contractId, int currentPage,
                           int pageSize)
        {
            if (_actionHistoryRepository.GetCommentActionByContractId(contractId).Result is not null)
            {
                var histories = await _actionHistoryRepository.GetCommentActionByContractId(contractId);
                IList<CommentResult> comments = new List<CommentResult>();
                var accesses = await _accessRepository.GetAccessByContractId(contractId);
                IList<User_Access> userAccesses = new List<User_Access>();
                foreach(var access in accesses)
                {
                    var userAccess = await _userAccessRepository.GetByAccessId(access.Id);
                    if(userAccess is not null)
                    {
                        userAccesses.Add(userAccess);
                    }
                }
                foreach (var commentHistory in histories)
                {
                    var comment = await _commentRepository.GetByActionHistoryId(commentHistory.Id);
                    var userRole = userAccesses.Where(ua => ua.UserId.Equals(commentHistory.UserId)).FirstOrDefault();
                    string accessRole = userRole.Access.AccessRole.ToString();
                    if (comment is not null)
                    {
                        var commentResult = new CommentResult()
                        {
                            Id = comment.Id,
                            Content = comment.Content,
                            ActionHistoryId = comment.ActionHistoryId,
                            ReplyId = comment.ReplyId,
                            Status = (int)comment.Status,
                            StatusString = comment.Status.ToString(),
                            AccessRole = accessRole,
                        };
                        commentResult.Long = AsTimeAgo(comment.ActionHistory.CreatedAt);
                        commentResult.CreatedAt = comment.ActionHistory.CreatedAt.ToString();
                        var user = await _userRepository.GetUser((int)comment.ActionHistory.UserId);
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
