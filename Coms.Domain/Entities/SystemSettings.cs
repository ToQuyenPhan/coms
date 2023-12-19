using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coms.Domain.Entities
{
    public class SystemSettings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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

        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(150)]
        public string BankAccount { get; set; }

        [Required]
        [MaxLength(100)]
        public string BankAccountNumber { get; set; }

        [Required]
        [MaxLength(250)]
        public string BankName { get; set; }
    }
}
