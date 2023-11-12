using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coms.Domain.Enum;

namespace Coms.Domain.Entities
{
    public class Attachment
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

        public int ContractId { get; set; }
        public virtual Contract? Contract { get; set; }
    }
}
