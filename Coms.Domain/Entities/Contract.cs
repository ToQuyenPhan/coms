using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coms.Domain.Enum;

namespace Coms.Domain.Entities
{
    public class Contract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string ContractName { get; set; }

        [Required]
        public int Version { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime UpdatedDate { get; set;}

        [Required]
        public DateTime EffectiveDate { get; set; }

        [Required]
        public DocumentStatus Status { get; set; }

        [Required]
        public int ContractTemplateID { get; set; }
        public virtual ContractTemplate ContractTemplate { get; set; }

        [Required]
        public int DocumentId { get; set; }
        public virtual Document Document { get; set; }
    }
}
