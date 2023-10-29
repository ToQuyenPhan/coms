using System.ComponentModel.DataAnnotations;

namespace Coms.Domain.Entities
{
    public class ContractAnnexTerm
    {
        [Required]
        public int ContractAnnexId { get; set; }
        public virtual ContractAnnex ContractAnnex { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
