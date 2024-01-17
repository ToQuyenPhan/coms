using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coms.Domain.Enum;

namespace Coms.Domain.Entities
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public int? ReplyId { get; set; }

        [Required]
        public CommentStatus Status { get; set; }

        [Required]
        public CommentType CommentType { get; set; }

        public int? ActionHistoryId { get; set; }
        public virtual ActionHistory ActionHistory { get; set; }    
    }
}
