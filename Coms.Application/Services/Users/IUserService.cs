﻿using Coms.Application.Services.Common;
using ErrorOr;

namespace Coms.Application.Services.Users
{
    public interface IUserService
    {
        Task<ErrorOr<PagingResult<UserResult>>> GetUsers(string fullName, string email, int? roleId, int? status,
                int currentPage, int pageSize);
        Task<ErrorOr<IList<UserResult>>> GetManagers();
        Task<ErrorOr<IList<UserResult>>> GetStaffs(int userId);
        Task<ErrorOr<UserResult>> GetUser(int id);
    }
}
