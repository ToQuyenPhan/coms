﻿using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IUserAccessRepository
    {
        Task<IList<User_Access>> GetYourAccesses(int userId);
        Task<User_Access> GetByAccessId(int accessId);
    }
}