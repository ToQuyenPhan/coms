using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Accesses;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.Flows
{
    public class FlowService : IFlowService
    {
        private readonly IFlowRepository _flowRepository;

        public FlowService(IFlowRepository flowRepository)
        {
            _flowRepository = flowRepository;
        }

        public async Task<ErrorOr<FlowResult>> AddFlow(int categoryId, int status)
        {
            try
            {
                var flow = new Flow
                {
                    ContractCategoryId = categoryId,
                    Status = (CommonStatus)status
                };
                await _flowRepository.AddFlow(flow);
                var created = await _flowRepository.GetFlowById(categoryId);
                var result = new FlowResult
                {
                    Id = created.Id,
                    ContractCategoryId = created.ContractCategoryId,
                    ContractCategory = created.ContractCategory.CategoryName,
                    Status = (int)created.Status,
                    StatusString = created.Status.ToString()
                };
                return result;

            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }
    }
}
