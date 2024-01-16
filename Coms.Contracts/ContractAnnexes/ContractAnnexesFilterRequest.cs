using Coms.Contracts.Common.Paging;

namespace Coms.Contracts.ContractAnnexes
{
    public class ContractAnnexesFilterRequest : PagingRequest
    {
        public string? ContractAnnexName { get; set; }
        public int? Status { get; set; }
        public bool IsYours { get; set; }
    }
}
