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
        public string ContractAnnexName { get; set; } = string.Empty;

        [Required]
        public int Version { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [Required]
        public DocumentStatus Status { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Link { get; set; }

        public int? TemplateId { get; set; }

        public int? ContractId { get; set; }
        public virtual Contract? Contract { get; set; }
        public virtual ICollection<ContractAnnexCost>? ContractAnnexCosts { get; set; }
    }
}
