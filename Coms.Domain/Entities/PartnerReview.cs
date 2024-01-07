using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coms.Domain.Enum;

namespace Coms.Domain.Entities
{
    public class PartnerReview
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime SendDate { get; set; }

        [Required]
        public DateTime ReviewAt { get; set; }

        [Required]
        public bool IsApproved { get; set; }

        [Required]
        public PartnerReviewStatus Status { get; set; }

        public int? PartnerId { get; set; }
        public virtual Partner Partner { get; set; }

        public int? UserId { get; set; }
        public virtual User User { get; set; }

        public int? ContractId { get; set; }
        public virtual Contract? Contract { get; set; }

        public int? ContractAnnexId { get; set; }
        public virtual ContractAnnex? ContractAnnex { get; set; }

        public int? LiquidationRecordId { get; set; }
        public virtual LiquidationRecord? LiquidationRecord { get; set; }
    }
}
