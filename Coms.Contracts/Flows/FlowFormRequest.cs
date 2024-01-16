using Coms.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Contracts.Flows
{
    public class FlowFormRequest
    {
        [Required]
        public int? ContractCategoryId { get; set; }
        [Required]
        public CommonStatus Status { get; set; }
    }
}
