using Coms.Contracts.Common.Paging;

namespace Coms.Contracts.Users
{
    public class UserFilterRequest : PagingRequest
    {
        public string? Fullname {  get; set; }
        public string? Email { get; set; }
        public int? RoleId { get; set; }
        public int? Status { get; set; }
    }
}
