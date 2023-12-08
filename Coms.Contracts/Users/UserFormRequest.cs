using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Contracts.Users
{
    public class UserFormRequest
    {
        [Required(ErrorMessage = "FullName is not null")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Email is not null")]
        [MaxLength(50)]
        [EmailAddress(ErrorMessage ="Email is not valid")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "UserName is not null")]
        [MaxLength(50)]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is not null")]
        public string Password { get; set; } = null!;

        public string? Image { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Dob { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}
