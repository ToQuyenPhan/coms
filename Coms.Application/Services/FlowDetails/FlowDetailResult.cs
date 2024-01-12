using Coms.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.FlowDetails
{
    public class FlowDetailResult
    {
        public int Id { get; set; }
        public FlowRole FlowRole { get; set; }
        public int? Order { get; set; }

        public int FlowID { get; set; }
        public int UserID { get; set; }
    }
}
