using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.PartnerReviews
{
    public class PartnerReviewResult
    {
        public int Id { get; set; }
        public DateTime SendDate { get; set; }
        public DateTime ReviewAt { get; set; }
        public bool IsApproved { get; set; }
        public int? PartnerId { get; set; }
        public string PartnerCompanyName { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public int ContractId { get; set; }
        public string ContractName { get; set; }
    }
}
