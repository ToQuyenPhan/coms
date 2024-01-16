using Coms.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.Flows
{
    public class FlowResult
    {
        public int Id { get; set; }

        public CommonStatus Status { get; set; }

        public int? ContractCategoryId { get; set; }
    }
}
