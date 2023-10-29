﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coms.Domain.Enum;

namespace Coms.Domain.Entities
{
    public class ActionHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public ActionType ActionType { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public int? UserId { get; set; }
        public virtual User User { get; set; }
    }
}
