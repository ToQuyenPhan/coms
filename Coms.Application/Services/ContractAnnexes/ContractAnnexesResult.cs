using Coms.Domain.Enum;

namespace Coms.Application.Services.ContractAnnexes
{
    public class ContractAnnexesResult
    {
        public int Id { get; set; }
        public string ContractAnnexName { get; set; }
        public int Version { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public DocumentStatus Status { get; set; }
        public int? ContractId { get; set; }
        public string Code { get; set; }
        public string Link { get; set; }
    }
}
