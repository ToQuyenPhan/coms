using Coms.Contracts.Common.Paging;

namespace Coms.Contracts.Attachments
{
    public class AttachmentRequest : PagingRequest
    {
        public int ContractId { get; set; }
        public int ContractAnnexId { get; set; }
        public int LiquidationeRecordId { get; set; }
    }
}
