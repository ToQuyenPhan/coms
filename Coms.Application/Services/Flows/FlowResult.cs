using Coms.Domain.Entities;
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
        public int Status { get; set; }
        public string StatusString { get; set; }
        public int? ContractCategoryId { get; set; }
        public string? ContractCategory { get; set; }
    }
}
