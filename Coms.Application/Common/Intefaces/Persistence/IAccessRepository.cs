using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IAccessRepository
    {
        Task<Access> GetAccessById(int id);
    }
}
