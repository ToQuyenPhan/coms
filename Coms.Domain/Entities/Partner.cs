using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coms.Domain.Enum;

namespace Coms.Domain.Entities
{
    public class Partner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Image { get; set; }

        [Required]
        [MaxLength(150)]
        public string Representative { get; set; }

        [Required]
        [MaxLength(50)]
        public string RepresentativePosition { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Code { get; set; }

        [Required]
        [MaxLength(30)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(255)]
        public string Address { get; set; }

        [Required]
        [MaxLength(255)]
        public string CompanyName { get; set; }

        [Required]
        [MaxLength(50)]
        public string TaxCode { get; set; }

        [Required]
        public PartnerStatus Status { get; set; }
    }
}
