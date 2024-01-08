using System.ComponentModel.DataAnnotations;

namespace Coms.Domain.Entities
{
    public class PartnerSign
    {
        [Required]
        public DateTime SignedAt { get; set; }

        public int? ContractId { get; set; }
        public virtual Contract? Contract { get; set; }

        public int? ContractAnnexId { get; set; }
        public virtual ContractAnnex? ContractAnnex { get; set; }

        public int? LiquidationRecordId { get; set; }
        public virtual LiquidationRecord? LiquidationRecord { get; set; }

        public int? PartnerId { get; set; }
        public virtual Partner Partner { get; set; }
    }
}
