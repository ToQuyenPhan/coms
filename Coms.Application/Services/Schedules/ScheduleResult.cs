using Coms.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Coms.Application.Services.Schedules
{
    public class ScheduleResult
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EndDateString { get; set; }
        public ScheduleType ScheduleType { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public int RemindBefore { get; set; }
        public ScheduleStatus Status { get; set; }
        public int UserId { get; set; }
        public string RemainTime {  get; set; }
    }
}
