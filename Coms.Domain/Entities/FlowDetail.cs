using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coms.Domain.Enum;

namespace Coms.Domain.Entities
{
    public class FlowDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public FlowRole FlowRole { get; set; }

        public int? UserId { get; set; }
        public virtual User? User { get; set; }

        public int? Order { get; set; }

        public int FlowID { get; set; }
        public virtual Flow? Flow { get; set; }
        public virtual ICollection<Contract_FlowDetail>? ContractFlowDetails { get; set; }
    }
}
