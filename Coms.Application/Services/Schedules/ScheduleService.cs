using Coms.Application.Common.Intefaces.Persistence;
using ErrorOr;
using Spire.Doc;

namespace Coms.Application.Services.Schedules
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task<ErrorOr<ScheduleResult>> GetTopSchedule(int userId)
        {
            var schedules = await _scheduleRepository.GetByUSerId(userId);
            if(schedules is not null)
            {
                schedules = schedules.OrderBy(s => s.EndDate).ToList();
                foreach (var schedule in schedules)
                {
                    if ((schedule.EndDate - DateTime.Now).Days < 3)
                    {
                        var scheduleResult = new ScheduleResult()
                        {
                            Id = schedule.Id,
                            StartDate = schedule.StartDate,
                            EndDate = schedule.EndDate,
                            ScheduleType = schedule.ScheduleType,
                            EventName = schedule.EventName,
                            Description = schedule.Description,
                            RemindBefore = schedule.RemindBefore,
                            Status = schedule.Status,
                            UserId = schedule.UserId,
                            EndDateString = schedule.EndDate.ToString("dd/MM/yyyy"),
                            RemainTime = AsTimeAgo(schedule.EndDate)
                        };
                        return scheduleResult;
                    }
                }
                return Error.NotFound("404", "Schedule not found!");
            }
            else
            {
                return Error.NotFound("404", "Not found any schedules!");
            }
        }

        public async Task<ErrorOr<ScheduleResult>> InactiveSchedule(int id)
        {
            try
            {
                var schedule = await _scheduleRepository.GetSchedule(id);
                if (schedule is not null)
                {
                    schedule.Status = Domain.Enum.ScheduleStatus.Inactive;
                    await _scheduleRepository.Update(schedule);
                    var scheduleResult = new ScheduleResult()
                    {
                        Id = schedule.Id,
                        StartDate = schedule.StartDate,
                        EndDate = schedule.EndDate,
                        ScheduleType = schedule.ScheduleType,
                        EventName = schedule.EventName,
                        Description = schedule.Description,
                        RemindBefore = schedule.RemindBefore,
                        Status = schedule.Status,
                        UserId = schedule.UserId,
                        EndDateString = schedule.EndDate.ToString("dd/MM/yyyy"),
                        RemainTime = AsTimeAgo(schedule.EndDate)
                    };
                    return scheduleResult;
                }
                else
                {
                    return Error.NotFound("404", "Schedule not found");
                }
            }catch(Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        private string AsTimeAgo(DateTime dateTime)
        {
            TimeSpan timeSpan = dateTime.Subtract(DateTime.Now);

            return timeSpan.TotalSeconds switch
            {
                <= 60 => $"{timeSpan.Seconds} seconds",

                _ => timeSpan.TotalMinutes switch
                {
                    <= 1 => "about a minute",
                    < 60 => $"about {timeSpan.Minutes} minutes",
                    _ => timeSpan.TotalHours switch
                    {
                        <= 1 => "about an hour ago",
                        < 24 => $"about {timeSpan.Hours} hours",
                        _ => timeSpan.TotalDays switch
                        {
                            <= 1 => "yesterday",
                            <= 30 => $"about {timeSpan.Days} days",

                            <= 60 => "about a month ago",
                            < 365 => $"about {timeSpan.Days / 30} months",

                            <= 365 * 2 => "about a year ago",
                            _ => $"about {timeSpan.Days / 365} years"
                        }
                    }
                }
            };
        }
    }
}
