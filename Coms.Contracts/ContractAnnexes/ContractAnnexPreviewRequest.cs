namespace Coms.Contracts.ContractAnnexes
{
    public class ContractAnnexPreviewRequest
    {
        public string[] Name { get; set; }
        public string[] Value { get; set; }
        public int ContractCategoryId { get; set; }
        public int TemplateType {  get; set; }
    }
}
