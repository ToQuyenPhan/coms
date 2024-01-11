using Coms.Application.Services.Flows;
using Coms.Application.Services.Services;
using Coms.Domain.Enum;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.FlowDetails
{
    public interface IFlowDetailService
    {   
        Task<ErrorOr<FlowDetailResult>> AddFlowDetail(FlowRole flowRole, int order, int flowId, int userId);
        Task<ErrorOr<FlowDetailResult>> GetFlowDetail(int id); 
        Task<ErrorOr<IList<FlowDetailResult>>> GetFlowDetailByFlowId(int flowId);
        Task<ErrorOr<FlowDetailResult>> UpdateFlowDetail(int id, FlowRole flowRole, int order, int flowId, int userId);
    }
}
