using Coms.Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Coms.Domain.Entities
{
    public class ContractAnnex
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string ContractAnnexName { get; set; }

        [Required]
        public int Version { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime UpdatedDate { get; set; }

        [Required]
        public DocumentStatus Status { get; set; }

        public int? ContractId { get; set; }
        public virtual Contract Contract { get; set; }

        [Required]
        public int ContractAnnexTemplateID { get; set; }
        public virtual ContractAnnexTemplate ContractAnnexTemplate { get; set; }

        [Required]
        public int DocumentId { get; set; }
        public virtual Document Document { get; set; }
    }
}
