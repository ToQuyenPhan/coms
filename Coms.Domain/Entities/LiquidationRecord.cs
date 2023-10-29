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
        public string LiquidationName { get; set; }

        [Required]
        public int Version { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DocumentStatus Status { get; set; }

        [Required]
        public int ContractId { get; set; }
        public Contract Contract { get; set; }

        [Required]
        public int LiquidationRecordTemplateId { get; set; }
        public LiquidationRecordTemplate LiquidationRecordTemplate { get; set;  }

        public int? DocumentId { get; set; }
        public virtual Document Document { get; set; }
    }
}
