using Coms.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Coms.Domain.Entities
{
    public class ApproveWorkflow
    {
        public int? AccessId { get; set; }
        public virtual Access Access { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public ApproveWorkflowStatus Status { get; set; }
    }
}
