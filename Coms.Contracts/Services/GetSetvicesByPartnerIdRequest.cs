using Coms.Contracts.Common.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Contracts.Services
{
    public class GetSetvicesByPartnerIdRequest : PagingRequest
    {
        public int? PartnerId { get; set; }
    }
}
