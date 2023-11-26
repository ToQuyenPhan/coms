using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Contracts;
using Coms.Application.Services.UserAccesses;
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
        private readonly IUserAccessRepository _userAccessRepository;

        public AccessService(IAccessRepository accessRepository,
            IContractRepository contractRepository,
            IUserAccessRepository userAccessRepository)
        {
            _accessRepository = accessRepository;
            _contractRepository = contractRepository;
            _userAccessRepository = userAccessRepository;
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
                    AccessRole = access.AccessRole.ToString(),
                    ContractName = contract.ContractName
                };
                return accessResult;

            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<AccessResult>> AddViewers(int[] users,int contractId)
        {
            try
            {
                var access = new Access
                {
                    ContractId = contractId,
                    AccessRole = AccessRole.Viewer
                };
                await _accessRepository.AddAccess(access);
                await _userAccessRepository.AddUserToUserAccess(users, access.Id);
                var contract = _contractRepository.GetContract(contractId).Result;
                var result = new AccessResult
                {
                    Id = access.Id,
                    ContractId= access.ContractId,
                    ContractName= contract.ContractName,
                    AccessRole = access.AccessRole.ToString()
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
