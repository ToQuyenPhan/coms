using ErrorOr;

namespace Coms.Application.Services.Authentication
{
    public interface IAuthenticationService
    {
        ErrorOr<AuthenticationResult> Login(string username, string password);
        ErrorOr<AuthenticationResult> EnterCode(string code);
        Task<ErrorOr<string>> SendEmail(int partnerId);
    }
}
