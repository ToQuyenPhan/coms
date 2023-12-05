using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coms.Domain.Enum;

namespace Coms.Domain.Entities
{
    public class Access
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public AccessRole AccessRole { get; set; }

        //public int? ContractId { get; set; }
        //public virtual Contract? Contract { get; set; }
        //public virtual ICollection<User_Access>? User_Accesses { get; set; }
        public virtual ICollection<ApproveWorkflow>? ApproveWorkflows { get; set; }
    }
}
