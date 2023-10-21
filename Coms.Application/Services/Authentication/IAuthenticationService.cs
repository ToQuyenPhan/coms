namespace Coms.Application.Services.Authentication
{
    public interface IAuthenticationService
    {
        AuthenticationResult Login(string username, string password);
    }
}
