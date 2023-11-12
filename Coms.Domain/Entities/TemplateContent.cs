using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Coms.Domain.Entities
{
    public class TemplateContent
    {
        public string? Content { get; set; }

        public int ContractId { get; set; }
        public virtual Contract? Contract { get; set; }

        public int TemplateFieldId { get; set; }
        public virtual TemplateField? TemplateField { get; set; }
    }
}
