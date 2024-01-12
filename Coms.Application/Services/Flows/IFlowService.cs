using Coms.Application.Services.FlowDetails;
using Coms.Domain.Enum;
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
        Task<ErrorOr<FlowResult>> AddFlow(CommonStatus status, int contractCategoryId);
    }
}
