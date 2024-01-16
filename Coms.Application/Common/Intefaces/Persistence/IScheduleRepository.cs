using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IScheduleRepository
    {
        Task Add(Schedule schedule);
        Task<IList<Schedule>?> GetByUSerId(int userId);
        Task Update(Schedule schedule);
        Task<Schedule?> GetSchedule(int id);
    }
}
