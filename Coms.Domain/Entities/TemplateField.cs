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
        public string FieldName { get; set; }

        [Required]
        public int PositionX { get; set; }

        [Required]
        public int PositionY { get; set; }

        [Required] 
        public FieldType FieldType { get; set; }

        public int? ContractTemplateId { get; set; }

        public virtual ContractTemplate ContractTemplate { get; set; }
    }
}
