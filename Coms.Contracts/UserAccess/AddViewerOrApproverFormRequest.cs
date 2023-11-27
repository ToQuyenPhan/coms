using Coms.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Contracts.UserAccess
{
    public class AddViewerOrApproverFormRequest
    {
        [Required(ErrorMessage = "UserId is not null")]
        public int[] Users { get; set; }
        [Required(ErrorMessage = "ContractId is not null")]
        public int ContractId { get; set; }
        
    }
}
