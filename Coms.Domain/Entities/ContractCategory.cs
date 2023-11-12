using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coms.Domain.Enum;

namespace Coms.Domain.Entities
{
    public class ContractCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string CategoryName { get; set; } = null!;

        [Required]
        public ContractCategoryStatus Status { get; set; }
        public virtual ICollection<Template>? Templates { get; set; }
    }
}
