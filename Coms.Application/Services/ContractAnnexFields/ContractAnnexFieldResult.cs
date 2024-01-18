namespace Coms.Application.Services.ContractAnnexFields
{
    public class ContractAnnexFieldResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsReadOnly { get; set; }
        public string? Content { get; set; }
        public string Type { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
    }
}
