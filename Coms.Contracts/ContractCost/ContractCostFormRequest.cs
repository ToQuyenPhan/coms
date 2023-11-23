using Coms.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Contracts.ContractCost
{
    public class ContractCostFormRequest
    {
        [Required(ErrorMessage = "ContractId is not null")]
        public int ContractId { get; set; }
        [Required(ErrorMessage = "ServiceId is not null")]
        public int ServiceId { get; set; }
    }
}
