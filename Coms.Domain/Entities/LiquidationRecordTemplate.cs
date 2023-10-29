using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coms.Domain.Enum;

namespace Coms.Domain.Entities
{
    public class LiquidationRecordTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string TemplateName { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime UpdatedDate { get; set; }

        [Required]
        public TemplateStatus Status { get; set; }

        public int? ContractCategoryId { get; set; }
        public virtual ContractCategory ContractCategory { get; set; }

        [Required]
        public int LiquidationTypeId { get; set; }
        public virtual LiquidationType LiquidationType { get; set; }
    }
}
