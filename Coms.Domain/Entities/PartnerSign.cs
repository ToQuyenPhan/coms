using System.ComponentModel.DataAnnotations;

namespace Coms.Domain.Entities
{
    public class PartnerSign
    {
        [Required]
        public DateTime SignedAt { get; set; }

        public int? DocumentId { get; set; }
        public virtual Document Document { get; set; }

        public int? PartnerId { get; set; }
        public virtual Partner Partner { get; set; }
    }
}
