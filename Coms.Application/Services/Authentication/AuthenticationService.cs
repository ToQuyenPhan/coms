using Coms.Application.Common.Intefaces.Authentication;

namespace Coms.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
        }   

        public AuthenticationResult Login(string username, string password)
        {
            //Create JWT Token
            Guid userId = Guid.NewGuid();
            var token = _jwtTokenGenerator.GenerateToken(userId, username);
            return new AuthenticationResult(Guid.NewGuid(), username, token);
        }
    }
}
