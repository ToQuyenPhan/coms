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

namespace Coms.Application.Services.UserAccesses
{
    public class UserAccessService :IUserAccessService
    {
        private readonly IUserAccessRepository _userAccessRepository;
        private readonly IAccessRepository _accessRepository;

        public UserAccessService(IUserAccessRepository userAccessRepository, IAccessRepository accessRepository)
        {
            _userAccessRepository = userAccessRepository;
            _accessRepository = accessRepository;
        }

        public async Task<ErrorOr<UserAccessResult>> AddUserAccess(int contractId, int userId, int accessRole)
        {
            try
            {
                var access = new Access
                {
                    ContractId = contractId,
                    AccessRole = (AccessRole)accessRole
                };
                await _accessRepository.AddAccess(access);
                var userAccess = new User_Access
                {
                    UserId = userId,
                    AccessId = access.Id,
                };
                await _userAccessRepository.AddUserAccess(userAccess);
                var result = new UserAccessResult
                {
                    UserId= userAccess.UserId,
                    //UserName = userAccess.User.FullName,
                    AccessId = userAccess.AccessId,
                    //AccessRole = userAccess.Access.AccessRole.ToString()
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
