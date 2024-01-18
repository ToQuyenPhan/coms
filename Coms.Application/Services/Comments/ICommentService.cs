using Coms.Application.Services.Common;
using ErrorOr;

namespace Coms.Application.Services.Comments
{
    public interface ICommentService
    {
        Task<ErrorOr<PagingResult<CommentResult>>> GetAllComments(int userId, int currentPage, int pageSize);
        Task<ErrorOr<CommentResult>> DismissComment(int id);
        Task<ErrorOr<PagingResult<CommentResult>>> GetContractComments(int contractId, int currentPage, int pageSize);
        Task<ErrorOr<CommentDetailResult>> GetCommentDetail(int id);
        Task<ErrorOr<CommentResult>> LeaveComment(int userId, int contractId, string content, int? replyId, 
            int commentType);
        Task<ErrorOr<CommentResult>> DeleteComment(int id);
        Task<ErrorOr<CommentResult>> EditComment(int id, string content);
        //get all comments of a contract annex
        Task<ErrorOr<PagingResult<CommentResult>>> GetContractAnnexComments(int contractAnnexId, int currentPage, int pageSize);
        Task<ErrorOr<CommentResult>> LeaveContractAnnexComment(int userId, int contractAnnexId, string content, int? replyId, int commentType);
    }
}
