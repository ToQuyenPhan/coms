using Coms.Application.Services.Common;
using ErrorOr;

namespace Coms.Application.Services.Comments
{
    public interface ICommentService
    {
        Task<ErrorOr<PagingResult<CommentResult>>> GetAllComments(int userId, int currentPage, int pageSize);
        Task<ErrorOr<CommentResult>> DismissComment(int id);
        Task<ErrorOr<PagingResult<CommentResult>>> GetContractComments(int contractId, int currentPage, int pageSize);
    }
}
