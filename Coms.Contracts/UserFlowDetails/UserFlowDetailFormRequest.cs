using Coms.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Contracts.UserFlowDetails
{
    public class UserFlowDetailFormRequest
    {
        [Required]
        public int Status { get; set; }
        [Required]
        public int FlowDetailId { get; set; }
        [Required]
        public int ContractId { get; set; }
        //[Required]
        //public int FlowDetailId { get; set; }
    }
}
