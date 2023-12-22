using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Coms.Domain.Entities
{
    public class LiquidationRecordFile
    {
        [Key]
        public Guid UUID { get; set; }

        [Required]
        public byte[] FileData { get; set; }

        [Required]
        public int FileSize { get; set; }

        [Required]
        public DateTime UploadedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int LiquidationRecordId { get; set; }
        public virtual LiquidationRecord LiquidationRecord { get; set; }
    }
}
