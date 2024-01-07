using Coms.Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Coms.Domain.Entities
{
    public class LiquidationRecordAttachment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public string FileLink { get; set; } = string.Empty;

        [Required]
        public DateTime UploadDate { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public AttachmentStatus Status { get; set; }

        public int LiquidationRecordId { get; set; }
        public virtual LiquidationRecord LiquidationRecord { get; set; }
    }
}
