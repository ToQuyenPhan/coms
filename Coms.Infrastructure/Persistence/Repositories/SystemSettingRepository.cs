using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class SystemSettingRepository : ISystemSettingsRepository
    {
        private readonly IGenericRepository<SystemSettings> _genericRepository;

        public SystemSettingRepository(IGenericRepository<SystemSettings> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<SystemSettings?> GetSystemSettings()
        {
            return await 
                    _genericRepository.FirstOrDefaultAsync(ss => ss.CompanyName.Equals("Hisoft"), null);
        }
    }
}
