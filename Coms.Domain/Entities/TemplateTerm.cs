using System.ComponentModel.DataAnnotations;

namespace Coms.Domain.Entities
{
    public class TemplateTerm
    {
        [Required]
        public int TemplateId { get; set; }
        public virtual Template? Template { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
