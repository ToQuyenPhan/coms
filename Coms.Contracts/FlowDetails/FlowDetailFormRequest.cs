using Coms.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Contracts.FlowDetails
{
    public class FlowDetailFormRequest
    {
        [Required(ErrorMessage = "UserId is not null")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "FlowRole is not null")]
        public FlowRole FlowRole { get; set; }

        [Required(ErrorMessage = "Order is not null")]
        public int Order { get; set; }

        [Required]
        public int FlowId { get; set; }
    }
}
