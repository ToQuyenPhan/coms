using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coms.Domain.Enum;

namespace Coms.Domain.Entities
{
    public class Flow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public CommonStatus Status { get; set; }

        public int? ContractCategoryId { get; set; }
        public virtual ContractCategory? ContractCategory { get; set; }

        public virtual ICollection<FlowDetail>? FlowDetails { get; set;}
    }
}
