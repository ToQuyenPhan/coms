using Coms.Contracts.Common.Paging;

namespace Coms.Contracts.UserFlowDetails
{
    public class UserFlowDetailRequest : PagingRequest
    {
        public int ContractId {  get; set; }
    }
}
