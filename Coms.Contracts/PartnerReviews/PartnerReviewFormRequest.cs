using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Contracts.PartnerReviews
{
    public class PartnerReviewFormRequest
    {
        public int PartnerId { get; set; }
        public int ContractId { get; set; }
    }
}
