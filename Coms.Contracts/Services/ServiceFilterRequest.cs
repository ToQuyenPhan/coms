using Coms.Contracts.Common.Paging;

namespace Coms.Contracts.Services
{
    public class ServiceFilterRequest : PagingRequest
    {
        public int? ContractCategoryId { get; set; }
        public string? ServiceName {  get; set; }
    }
}
