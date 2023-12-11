using Coms.Application.Services.Accesses;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.Flows
{
    public interface IFlowService
    {
        Task<ErrorOr<FlowResult>> AddFlow(int categoryId, int status);
    }
}
