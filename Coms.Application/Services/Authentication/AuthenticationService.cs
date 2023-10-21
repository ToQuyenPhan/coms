namespace Coms.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        public AuthenticationResult Login(string username, string password)
        {
            return new AuthenticationResult(Guid.NewGuid(), username, "token");
        }
    }
}
