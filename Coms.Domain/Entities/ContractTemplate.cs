using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coms.Domain.Enum;

namespace Coms.Domain.Entities
{
    public class ContractTemplate
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

        [Required]
        public int ContractCategoryId { get; set; }
        public virtual ContractCategory ContractCategory { get; set; }
    }
}
