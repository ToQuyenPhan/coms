using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.FlowDetails;
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

        public async Task<ErrorOr<FlowResult>> AddFlow(CommonStatus status, int contractCategoryId)
        {
            try
            {
                var isExist = await _flowRepository.GetByContractCategoryId(contractCategoryId);
                if (isExist is not null)
                {
                    return Error.Failure("Flow already exists in the contract category!");
                }
                var flow = new Flow
                {
                    ContractCategoryId = contractCategoryId,
                    Status = status
                };
                await _flowRepository.AddFlow(flow);
                var created = _flowRepository.GetByContractCategoryId((int)flow.ContractCategoryId).Result;
                var result = new FlowResult
                {
                    Id = flow.Id,
                    Status = flow.Status,
                    ContractCategoryId = flow.ContractCategoryId 
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
