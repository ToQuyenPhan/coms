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
        private readonly IAproveWorkflowRepository _aproveWorkflowRepository;

        public AccessService(IAccessRepository accessRepository,
            IContractRepository contractRepository,
            IUserAccessRepository userAccessRepository,
            IAproveWorkflowRepository aproveWorkflowRepository)
        {
            _accessRepository = accessRepository;
            _contractRepository = contractRepository;
            _userAccessRepository = userAccessRepository;
            _aproveWorkflowRepository = aproveWorkflowRepository;
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

        public async Task<ErrorOr<IList<AccessResult>>> AddViewers(int[] users, int contractId)
        {
            try
            {
                var results = new List<AccessResult>();
                if (users.Any())
                {
                    for (int i = 0; i < users.Length; i++)
                    {
                        var access = new Access
                        {
                            ContractId = contractId,
                            AccessRole = AccessRole.Viewer
                        };
                        await _accessRepository.AddAccess(access);
                        var userAccess = new User_Access
                        {
                            AccessId = access.Id,
                            UserId = users[i]
                        };
                        await _userAccessRepository.AddUserAccess(userAccess);
                        var contract = _contractRepository.GetContract(contractId).Result;
                        var result = new AccessResult
                        {
                            Id = access.Id,
                            ContractId = access.ContractId,
                            ContractName = contract.ContractName,
                            AccessRole = access.AccessRole.ToString()
                        };
                        results.Add(result);
                    }
                }
                return results;

            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }
        public async Task<ErrorOr<IList<AccessResult>>> AddApproves(int[] users, int contractId)
        {
            try
            {
                var results = new List<AccessResult>();
                if (users.Any())
                {
                    for (int i = 0; i < users.Length; i++)
                    {
                        var access = new Access
                        {
                            ContractId = contractId,
                            AccessRole = AccessRole.Approver
                        };
                        await _accessRepository.AddAccess(access);
                        var userAccess = new User_Access
                        {
                            AccessId = access.Id,
                            UserId = users[i]
                        };
                        await _userAccessRepository.AddUserAccess(userAccess);
                        var approveWF = new ApproveWorkflow
                        {
                            AccessId=access.Id,
                            Order = i+1,
                            Status = ApproveWorkflowStatus.Waiting
                        };
                        await _aproveWorkflowRepository.AddUserAccess(approveWF);
                        var contract = _contractRepository.GetContract(contractId).Result;
                        var result = new AccessResult
                        {
                            Id = access.Id,
                            ContractId = access.ContractId,
                            ContractName = contract.ContractName,
                            AccessRole = access.AccessRole.ToString()
                        };
                        results.Add(result);
                    }
                }
                    return results;

                }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }
    }
}
