using Coms.Contracts.Common.Paging;

namespace Coms.Contracts.LiquidationRecords
{
    public class LiquidationRecordFilterRequest : PagingRequest
    {
        public string? LiquidationRecordName { get; set; }
        public int? Status { get; set; }
    }
}
