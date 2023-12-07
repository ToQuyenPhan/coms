using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //add partner
        public async Task AddPartner(Partner partner)
        {
            await _genericRepository.CreateAsync(partner);
        }
    }
}
