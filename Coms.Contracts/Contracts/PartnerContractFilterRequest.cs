using Coms.Contracts.Common.Paging;

namespace Coms.Contracts.Contracts
{
    public class PartnerContractFilterRequest : PagingRequest
    {
        public string? ContractName { get; set; }
        public string? Code { get; set; }
        public bool IsApproved { get; set; }
    }
}
