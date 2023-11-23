using Coms.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Contracts.ActionHistories
{
    public class ActionHistoryFormRequest
    {

        [Required(ErrorMessage = "ContractId is not null")]
        public int ContractId { get; set; }

        [Required(ErrorMessage = "ActionType is not null")]
        [EnumDataType(typeof(ActionType))]
        public int ActionType { get; set; }
    }
}
