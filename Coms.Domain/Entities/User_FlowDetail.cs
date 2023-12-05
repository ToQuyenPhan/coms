using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coms.Domain.Enum;

namespace Coms.Domain.Entities
{
    public class User_FlowDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public FlowDetailStatus Status { get; set; }

        public int UserId { get; set; }
        public virtual User? User { get; set; }
        
        public int ContractId { get; set; }
        public virtual Contract? Contract { get; set; }

        public int FlowDetailId { get; set; }
        public virtual FlowDetail? FlowDetail { get; set; }
    }
}
