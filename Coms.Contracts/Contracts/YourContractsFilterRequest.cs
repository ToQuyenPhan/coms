using Coms.Contracts.Common.Paging;

namespace Coms.Contracts.Contracts
{
    public class YourContractsFilterRequest : PagingRequest
    {
        public string? ContractName { get; set; }
        public string? CreatorName { get; set; }
        public int? Status { get; set; }
    }
}
