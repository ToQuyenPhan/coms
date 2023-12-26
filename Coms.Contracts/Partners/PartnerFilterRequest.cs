using Coms.Contracts.Common.Paging;

namespace Coms.Contracts.Partners
{
    public class PartnerFilterRequest : PagingRequest
    {
        public int? PartnerId { get; set; }
        public string? Pepresentative { get; set; }
        public string? CompanyName { get; set; }
        public int? Status { get; set; }
    }
}
