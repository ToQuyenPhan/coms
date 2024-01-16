using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Coms.Domain.Entities
{
    public class LiquidationRecordField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string FieldName { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public int? LiquidationRecordId { get; set; }
        public virtual LiquidationRecord? LiquidationRecord { get; set; }
    }
}
