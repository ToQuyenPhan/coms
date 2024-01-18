using Coms.Contracts.Common.Paging;

namespace Coms.Contracts.ContractAnnexes
{
    public class PartnerContractAnnexesFilterRequest : PagingRequest
    {
        public string? ContractAnnexName { get; set; }
        public string? Code { get; set; }
        public int? Version { get; set; }
        public int DocumentStatus { get; set; }
        public bool IsApproved { get; set; }
    }
}
