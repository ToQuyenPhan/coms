namespace Coms.Contracts.Templates
{
    public class TemplateFormRequest
    {
        public string TemplateName { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string TemplateLink { get; set; }
        public int ContractCategoryId { get; set; }
        public int TemplateTypeId { get; set; }
    }
}
