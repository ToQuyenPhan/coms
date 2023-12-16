using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByActionHistoryId(int actionHistoryId);
        Task UpdateComment(Comment comment);
        Task<Comment?> GetComment(int id);
    }
}
