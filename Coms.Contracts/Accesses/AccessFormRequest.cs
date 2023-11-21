using Coms.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Coms.Contracts.Accesses
{
    public class AccessFormRequest
    {
        public int ContractId { get; set; }

        [EnumDataType(typeof(AccessRole))]
        public AccessRole Status { get; set; }
    }
}
