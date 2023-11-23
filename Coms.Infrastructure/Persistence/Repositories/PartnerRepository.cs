using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
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

        public async Task<Partner> GetPartner(int id)
        {
            return await _genericRepository.FirstOrDefaultAsync(c => c.Id.Equals(id), null);
        }

    }
}
