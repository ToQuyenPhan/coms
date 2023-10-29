using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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

        public int? PartnerId { get; set; }
        public virtual Partner Partner { get; set; }

        public int? SenderId { get; set; }
        public virtual User User { get; set; }
    }
}
