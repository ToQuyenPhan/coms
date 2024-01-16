using Coms.Contracts.Common.Paging;

namespace Coms.Contracts.Contracts
{
    public class ContractStatisticFilterRequest : PagingRequest
    {
        public string? ContractName { get; set; }
        public string? Code { get; set; }
        public int Status { get; set; }
        public int ServiceId { get; set; }
        public int PartnerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
