using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coms.Domain.Enum;

namespace Coms.Domain.Entities
{
    public class Template
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string TemplateName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime UpdatedDate { get; set; }

        [Required]
        public TemplateStatus Status { get; set; }

        public string TemplateLink { get; set; }

        [Required]
        public int ContractCategoryId { get; set; }
        public virtual ContractCategory? ContractCategory { get; set; }

        public int TemplateTypeId { get; set; }
        public virtual TemplateType? TemplateTypes { get; set; }
        public virtual ICollection<Contract>? Contracts { get; set; }
        public virtual ICollection<TemplateTerm>? TemplateTerms { get; set; }
        public virtual ICollection<TemplateField>? TemplateFields { get; set; }
    }
}
