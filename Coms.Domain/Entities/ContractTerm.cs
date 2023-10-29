using System.ComponentModel.DataAnnotations;

namespace Coms.Domain.Entities
{
    public class ContractTerm
    {
        [Required]
        public int ContractId { get; set; }
        public virtual Contract Contract { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
