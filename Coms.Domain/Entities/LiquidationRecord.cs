using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coms.Domain.Enum;

namespace Coms.Domain.Entities
{
    public class LiquidationRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string LiquidationName { get; set; } = string.Empty;

        [Required]
        public int Version { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [Required]
        public DocumentStatus Status { get; set; }

        [Required]
        public int ContractId { get; set; }
        public Contract? Contract { get; set; }
        public virtual ICollection<LiquidationRecordTerm>? LiquidationRecordTerms { get; set; }
    }
}
