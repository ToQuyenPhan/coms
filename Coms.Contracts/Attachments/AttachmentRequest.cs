using Coms.Contracts.Common.Paging;

namespace Coms.Contracts.Attachments
{
    public class AttachmentRequest : PagingRequest
    {
        public int ContractId {  get; set; }
    }
}
