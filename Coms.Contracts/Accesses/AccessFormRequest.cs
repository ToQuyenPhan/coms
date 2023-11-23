using Coms.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Coms.Contracts.Accesses
{
    public class AccessFormRequest
    {
        [Required(ErrorMessage = "ContractId is not null")]
        public int ContractId { get; set; }
        [Required(ErrorMessage = "Status is not null")]
        [EnumDataType(typeof(AccessRole))]
        public AccessRole Status { get; set; }
    }
}
