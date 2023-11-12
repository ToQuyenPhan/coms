using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Coms.Domain.Entities
{
    public class ContractCost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? ServiceId { get; set; }
        public virtual Service? Service { get; set; }

        public int? ContractId { get; set; }
        public virtual Contract? Contract { get; set; }
        public virtual ICollection<ContractAnnexCost>? ContractAnnexCosts { get; set;}
    }
}
