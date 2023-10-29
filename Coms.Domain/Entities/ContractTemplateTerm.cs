using System.ComponentModel.DataAnnotations;

namespace Coms.Domain.Entities
{
    public class ContractTemplateTerm
    {
        [Required]
        public int ContractTemplateId { get; set; }
        public virtual ContractTemplate ContractTemplate { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
