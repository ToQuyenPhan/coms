using Coms.Domain.Enum;


namespace Coms.Application.Services.LiquidationRecords
{
    public class LiquidationRecordsResult
    {
        public int Id { get; set; }
        public string LiquidationName { get; set; }
        public int Version { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateString { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateString { get; set; }
        public DocumentStatus Status { get; set; }
        public string StatusString { get; set; }
        public int? ContractId { get; set; }
        public string Link { get; set; }
    }
}
