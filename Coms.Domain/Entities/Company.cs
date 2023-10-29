using System.ComponentModel.DataAnnotations;

namespace Coms.Domain.Entities
{
    public class Company
    {
        [Required]
        public string CompanyName { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        [MaxLength(255)]
        public string Address { get; set; }

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(20)]
        public string Hotline { get; set; }

        [Required]
        [MaxLength(50)]
        public string TaxCode { get; set; }
    }
}
