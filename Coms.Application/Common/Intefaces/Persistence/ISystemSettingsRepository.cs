using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface ISystemSettingsRepository
    {
        Task<SystemSettings?> GetSystemSettings();
        Task Update(SystemSettings systemSettings);
    }
}
