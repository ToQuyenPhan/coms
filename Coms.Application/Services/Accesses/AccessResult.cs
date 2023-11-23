using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.Accesses
{
    public class AccessResult
    {
        public int Id { get; set; }
        public int? ContractId { get; set; }
        public string ContractName { get; set; }
        public string AccessRole { get; set; }
    }
}
