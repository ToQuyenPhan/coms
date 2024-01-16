using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.ActionHistories;
using Coms.Application.Services.ContractCategories;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.ContractCosts
{
    public class ContractCostService : IContractCostService
    {
        private readonly IContractCostRepository _contractCostRepository;

        public ContractCostService(IContractCostRepository contractCostRepository)
        {
            _contractCostRepository = contractCostRepository;
        }
        public async Task<ErrorOr<ContractCostResult>> AddContractCost(int contractId, int serviceId)
        {
            try
            {
                var contractCost = new ContractCost
                {
                    ContractId = contractId,
                    ServiceId = serviceId
                };
                await _contractCostRepository.AddContractCost(contractCost);
                var created = _contractCostRepository.GetContractCost(contractCost.Id).Result;
                var result = new ContractCostResult
                { 
                    Id = contractCost.Id,
                    ContractId = contractId,
                    ContractName=created.Contract.ContractName,
                    ServiceId=serviceId,
                    ServiceName = created.Service.ServiceName
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
