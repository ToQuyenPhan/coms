using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class UserFlowDetailsRepository : IUserFlowDetailsRepository
    {
        private readonly IGenericRepository<User_FlowDetail> _genericRepository;

        public UserFlowDetailsRepository(IGenericRepository<User_FlowDetail> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<IList<User_FlowDetail>?> GetUserFlowDetailsByUserId(int userId)
        {
            var list = await _genericRepository.WhereAsync(ufd => ufd.UserId.Equals(userId), 
                new System.Linq.Expressions.Expression<Func<User_FlowDetail, object>>[] { ufd => ufd.User,
                        ufd => ufd.Contract, ufd => ufd.FlowDetail });
            return (list.Count() > 0) ? list : null;
        }
    }
}
