using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coms.Domain.Entities
{
    public class ContractField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string FieldName { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public int? ContractId { get; set; }
        public virtual Contract? Contract { get; set; }

    }
}
