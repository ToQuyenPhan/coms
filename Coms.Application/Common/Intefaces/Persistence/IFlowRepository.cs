using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IFlowRepository
    {
        Task<Flow> GetFlowById(int id);
        Task AddFlow(Flow flow);
    }
}
