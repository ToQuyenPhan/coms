using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coms.Domain.Enum;

namespace Coms.Domain.Entities
{
    public class TemplateField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string FieldName { get; set; } = string.Empty;

        [Required]
        public int PositionX { get; set; }

        [Required]
        public int PositionY { get; set; }

        [Required] 
        public FieldType FieldType { get; set; }

        public int? TemplateId { get; set; }

        public virtual Template? Template { get; set; }
        public virtual ICollection<TemplateContent>? TemplateContents { get; set; }
    }
}
