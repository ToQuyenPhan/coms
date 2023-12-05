using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using Coms.Domain.Enum;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class AccessRepository : IAccessRepository
    {
        private readonly IGenericRepository<Access> _genericRepository;

        public AccessRepository(IGenericRepository<Access> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        //public async Task<Access> GetAccessById(int id)
        //{
        //    return await _genericRepository.FirstOrDefaultAsync(a => a.Id.Equals(id), 
        //            new System.Linq.Expressions.Expression<Func<Access, object>>[]
        //            { a => a.Contract});
        //}

        //public async Task AddAccess(Access access)
        //{
        //    await _genericRepository.CreateAsync(access);
        //}

        //public async Task<Access?> GetManagerAccess(int id)
        //{
        //    return await _genericRepository.FirstOrDefaultAsync(a => a.Id.Equals(id) 
        //        && a.AccessRole.Equals(AccessRole.Approver),
        //        new System.Linq.Expressions.Expression<Func<Access, object>>[]
        //            { a => a.Contract});
        //}

        //public async Task<IList<Access>?> GetAccessByContractId(int contractId)
        //{
        //    var list = await _genericRepository.WhereAsync(a => a.ContractId.Equals(contractId),
        //            new System.Linq.Expressions.Expression<Func<Access, object>>[]
        //            { a => a.Contract});
        //    return (list.Count() > 0) ? list : null;
        //}
    }
}
