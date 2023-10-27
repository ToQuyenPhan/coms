using Coms.Application.Common.Intefaces.Authentication;
using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;

namespace Coms.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public AuthenticationResult Login(string username, string password)
        {
            // Validate the user exist
            if(_userRepository.GetUserByUsername(username) is not User user)
            {
                throw new Exception("Your username is incorrect!");
            }
            // Validate the password is correct
            if(user.Password != password)
            {
                throw new Exception("Invalid password!");
            }
            //Create JWT Token
            var token = _jwtTokenGenerator.GenerateToken(new User { Username = "string", Password = "string"});
            return new AuthenticationResult(user, token);
        }
    }
}
