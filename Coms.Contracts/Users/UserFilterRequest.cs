using Coms.Contracts.Common.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Contracts.Users
{
    public class UserFilterRequest : PagingRequest
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public int? Role { get; set; }
        public int? Status { get; set; }
    }
}
