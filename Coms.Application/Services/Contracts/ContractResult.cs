namespace Coms.Application.Services.Contracts
{
    public class ContractResult
    {
        public int Id { get; set; }
        public string ContractName { get; set; }
        public int Version { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateString { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateString { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string EffectiveDateString { get; set; }
        public int Status { get; set; }
        public string StatusString { get; set; }
        public int TemplateID { get; set; }
        public int? CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string CreatorEmail { get; set; }
        public string CreatorImage { get; set; }
        public int? PartnerId { get; set; }
        public string PartnerName { get; set;}
        public string Code { get; set; }
        public string Link { get; set; }
    }
}
