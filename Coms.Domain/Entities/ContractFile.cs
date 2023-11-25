using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Coms.Domain.Entities
{
    public class ContractFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FileName { get; set; }

        [Required]
        [MaxLength(10)]
        public string Extension { get; set; }

        [Required]
        [MaxLength(200)]
        public string ContentType { get; set; }

        [Required]
        public byte[] FileData { get; set; }

        [Required]
        public int FileSize { get; set; }

        [Required]
        public DateTime UploadedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int ContractId { get; set; }
        public virtual Contract Contract { get; set; }
    }
}
