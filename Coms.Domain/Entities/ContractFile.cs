using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Coms.Domain.Entities
{
    public class ContractFile
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

        public int ContractId { get; set; }
        public virtual Contract Contract { get; set; }

        public int? X { get; set; }
        public int? Y { get; set; }
    }
}
