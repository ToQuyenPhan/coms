using System.ComponentModel.DataAnnotations;

namespace Coms.Domain.Entities
{
    public class PartnerSign
    {
        [Required]
        public DateTime SignedAt { get; set; }

        public int? ContractId { get; set; }
        public virtual Contract? Contract { get; set; }

        public int? PartnerId { get; set; }
        public virtual Partner Partner { get; set; }
    }
}
