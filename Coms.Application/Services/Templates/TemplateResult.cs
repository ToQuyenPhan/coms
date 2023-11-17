using Coms.Domain.Entities;
using Coms.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Coms.Application.Services.Templates
{
    public class TemplateResult
    {
        public int Id { get; set; }
        public string TemplateName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateString { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateString { get; set; }
        public int Status { get; set; }
        public string StatusString { get; set; }
        public string TemplateLink { get; set; }
        public int ContractCategoryId { get; set; }
        public string ContractCategoryName { get; set; }
        public int TemplateTypeId { get; set; }
        public string TemplateTypeName { get; set; }
    }
}
