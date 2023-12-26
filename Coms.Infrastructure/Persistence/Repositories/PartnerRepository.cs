using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqKit;
using System.Linq.Expressions;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class PartnerRepository : IPartnerRepository
    {
        private readonly IGenericRepository<Partner> _genericRepository;

        public PartnerRepository(IGenericRepository<Partner> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<Partner?> GetPartner(int id)
        {
            return await _genericRepository.FirstOrDefaultAsync(c => c.Id.Equals(id), null);
        }

        public async Task<IList<Partner>?> GetActivePartners()
        {
            var list = await _genericRepository.WhereAsync(
                    cc => cc.Status.Equals(PartnerStatus.Active), null);
            return (list.Count > 0) ? list : null;
        }

        public async Task<Partner?> GetPartnerByCode(string code)
        {
            return await _genericRepository.FirstOrDefaultAsync(c => c.Code.Equals(code), null);
        }

        //get all partners
        public async Task<PagingResult<Partner>?> GetPartners(int? partnerId, string pepresentative, string companyName, int? status, int currentPage, int pageSize)
        {
            IList<Partner> query = await _genericRepository.WhereAsync(BuildExpression(partnerId, pepresentative, companyName, status), null);
            int totalCount = query.Count();
            IList<Partner> list = await _genericRepository.WhereAsyncWithFilter(BuildExpression(partnerId, pepresentative, companyName, status),
                                   new System.Linq.Expressions.Expression<Func<Partner, object>>[] { },
                                                      currentPage, pageSize);
            return (list.Count() > 0) ? new PagingResult<Partner>(list, totalCount, currentPage, pageSize) : null;
        }


        //get partner by email
        public async Task<Partner?> GetPartnerByEmail(string email)
        {
            return await _genericRepository.FirstOrDefaultAsync(c => c.Email.Equals(email), null);
        }

        //add partner
        public async Task AddPartner(Partner partner)
        {
            await _genericRepository.CreateAsync(partner);
        }

        //update partner
        public async Task UpdatePartner(Partner partner)
        {
            await _genericRepository.UpdateAsync(partner);
        }

        private Expression<Func<Partner, bool>> BuildExpression(int? partnerId, string pepresentative, string companyName, int? status)
        {
            ExpressionStarter<Partner> predicate = PredicateBuilder.New<Partner>(true);
            if (partnerId > 0)
            {
                predicate = predicate.And(t => t.Id.Equals(partnerId));
            }
            if (!string.IsNullOrEmpty(pepresentative))
            {
                predicate = predicate.And(t => t.Representative.Contains(pepresentative.Trim()));
            }
            if (!string.IsNullOrEmpty(companyName))
            {
                predicate = predicate.And(t => t.CompanyName.Contains(companyName.Trim()));
            }
            if (status >= 0)
            {
                predicate = predicate.And(t => t.Status.Equals((PartnerStatus)status));
            }
            return predicate;
        }
    }
}
