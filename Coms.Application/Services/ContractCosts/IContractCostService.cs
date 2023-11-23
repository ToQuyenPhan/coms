using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.ContractCosts
{
    public interface IContractCostService
    {
        Task<ErrorOr<ContractCostResult>> AddContractCost(int contractId, int serviceId);
    }
}
