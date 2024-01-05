using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coms.Domain.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string Password { get; set; } = null!;

        [Required]
        [MaxLength(30)]
        public string Phone { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Position { get; set; } = null!;

        public string? Image { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Dob { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        public int RoleId { get; set; }
        public virtual Role? Role { get; set; }
        public virtual ICollection<ActionHistory>? ActionHistories { get; set; }
        //public virtual ICollection<User_Access>? UserAccesses { get; set; }
        public virtual ICollection<FlowDetail>? FlowDetails { get; set; }
        public virtual ICollection<PartnerReview>? PartnerReviews { get; set; }
        public virtual ICollection<Template> Templates { get; set; }
    }
}
