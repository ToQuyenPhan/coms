using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IUserAccessRepository
    {
        //Task<IList<User_Access>?> GetYourAccesses(int userId);
        //Task<User_Access?> GetByAccessId(int accessId);
        Task AddUserAccess(User_Access userAccess);
        Task AddUserToUserAccess(int[] users, int accessId);
    }
}
