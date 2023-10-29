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
        public string FileName { get; set; }

        [Required]
        public string FileLink { get; set; }

        [Required]
        public DateTime UploadDate { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public AttachmentStatus Status { get; set; }
    }
}
