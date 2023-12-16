using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using Coms.Domain.Enum;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IGenericRepository<Comment> _genericRepository;

        public CommentRepository(IGenericRepository<Comment> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<Comment?> GetByActionHistoryId(int actionHistoryId)
        {
            return await _genericRepository.FirstOrDefaultAsync(c => c.ActionHistoryId.Equals(actionHistoryId) && 
                c.Status.Equals(CommentStatus.Active), 
                new System.Linq.Expressions.Expression<Func<Comment, object>>[] {  c => c.ActionHistory });
        }

        public async Task UpdateComment(Comment comment)
        {
            await _genericRepository.UpdateAsync(comment);
        }

        public async Task<Comment?> GetComment(int id)
        {
            return await _genericRepository.FirstOrDefaultAsync(c => c.Id.Equals(id), 
                    new System.Linq.Expressions.Expression<Func<Comment, object>>[] { c => c.ActionHistory });
        }
    }
}
