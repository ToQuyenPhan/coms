using System.ComponentModel.DataAnnotations;

namespace Coms.Domain.Entities
{
    public class LiquidationRecordTerm
    {
        [Required]
        public int LiquidationRecordId { get; set; }
        public virtual LiquidationRecord LiquidationRecord { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
