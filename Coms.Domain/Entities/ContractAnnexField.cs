using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Coms.Domain.Entities
{
    public class ContractAnnexField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string FieldName { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public int? ContractAnnexId { get; set; }
        public virtual ContractAnnex? ContractAnnex { get; set; }
    }
}
