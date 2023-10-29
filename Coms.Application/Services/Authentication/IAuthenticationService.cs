﻿using ErrorOr;

namespace Coms.Application.Services.Authentication
{
    public interface IAuthenticationService
    {
        ErrorOr<AuthenticationResult> Login(string username, string password);
    }
}
