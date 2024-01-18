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
            return _genericRepository.GetAll().Take(1).FirstOrDefault();
        }

        public async Task Update(SystemSettings systemSettings)
        {
            await _genericRepository.UpdateAsync(systemSettings);
        }
    }
}
