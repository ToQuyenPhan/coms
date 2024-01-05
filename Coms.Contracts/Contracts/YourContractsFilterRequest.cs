using Coms.Contracts.Common.Paging;

namespace Coms.Contracts.Contracts
{
    public class YourContractsFilterRequest : PagingRequest
    {
        public string? ContractName { get; set; }
        public string? Code { get; set; }
        public int? Status { get; set; }
        public int? Version { get; set; }
        public bool IsYours { get; set; }
    }
}
