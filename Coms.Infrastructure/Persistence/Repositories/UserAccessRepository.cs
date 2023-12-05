using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class UserAccessRepository : IUserAccessRepository
    {
        private readonly IGenericRepository<User_Access> _genericRepository;

        public UserAccessRepository(IGenericRepository<User_Access> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        //public async Task<IList<User_Access>?> GetYourAccesses(int userId)
        //{
        //    var list = await _genericRepository.WhereAsync(ua => ua.UserId.Equals(userId), 
        //        new System.Linq.Expressions.Expression<Func<User_Access, object>>[] { 
        //            ua => ua.User });
        //    return (list.Count() > 0) ? list : null;
        //}

        //public async Task<User_Access?> GetByAccessId(int accessId)
        //{
        //    return await _genericRepository.FirstOrDefaultAsync(ua => ua.AccessId.Equals(accessId),
        //        new System.Linq.Expressions.Expression<Func<User_Access, object>>[] {
        //            ua => ua.User, ua => ua.Access });
        //}
        public async Task AddUserAccess(User_Access userAccess)
        {
            await _genericRepository.CreateAsync(userAccess);
        }

        public async Task AddUserToUserAccess(int[] users, int accessId)
        {
            if (users.Any())
            {
                for (int i = 0; i < users.Length; i++)
                {
                    //var userAccess = new User_Access
                    //{
                    //    AccessId = accessId,
                    //    UserId = int.Parse(users[i].ToString())
                    //};
                    //await _genericRepository.CreateAsync(userAccess);
                }
            }
        }
    }
}
