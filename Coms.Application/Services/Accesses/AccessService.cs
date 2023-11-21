using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Contracts;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.Accesses
{
    public class AccessService : IAccessService
    {
        private readonly IAccessRepository _accessRepository;
        private readonly IContractRepository _contractRepository;

        public AccessService(IAccessRepository accessRepository,
            IContractRepository contractRepository)
        {
            _accessRepository = accessRepository;
            _contractRepository = contractRepository;
        }

        public async Task<ErrorOr<AccessResult>> AddAccess(int contractId, int accessRole)
        {
            try
            {
                var access = new Access
                {
                    ContractId = contractId,
                    AccessRole = (AccessRole)accessRole
                };
                await _accessRepository.AddAccess(access);
                var contract = _contractRepository.GetContract(contractId).Result;
                var accessResult = new AccessResult
                {
                    Id = access.Id,
                    ContractId = access.ContractId,
                    AccessRole = accessRole,
                    ContractName = contract.ContractName
                };
                return accessResult;

            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }
    }
}
