using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coms.Domain.Enum;

namespace Coms.Domain.Entities
{
    public class Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public ScheduleType ScheduleType { get; set; }

        [Required]
        [MaxLength(100)]
        public string EventName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int RemindBefore { get; set; }

        [Required]
        public ScheduleStatus Status { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
