using ErrorOr;

namespace Coms.Application.Services.Schedules
{
    public interface IScheduleService
    {
        Task<ErrorOr<ScheduleResult>> GetTopSchedule(int userId);
        Task<ErrorOr<ScheduleResult>> InactiveSchedule(int id);
    }
}
