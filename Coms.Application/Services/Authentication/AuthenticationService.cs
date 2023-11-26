using Coms.Application.Common.Intefaces.Authentication;
using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Common.Errors;
using Coms.Domain.Entities;
using ErrorOr;

namespace Coms.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;
        private readonly IPartnerRepository _partnerRepository;

        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, 
            IUserRepository userRepository, IPartnerRepository partnerRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
            _partnerRepository = partnerRepository;
        }

        public ErrorOr<AuthenticationResult> Login(string username, string password)
        {
            // Validate the user exist
            if (_userRepository.GetUserByUsername(username).Result is not User user)
            {
                return Errors.User.IncorrectUsername;
            }
            // Validate the password is correct
            if(user.Password != password)
            {
                return Errors.User.IncorrectPassword;
            }
            //Create JWT Token
            var token = _jwtTokenGenerator.GenerateToken(user);
            return new AuthenticationResult(token);
        }

        public ErrorOr<AuthenticationResult> EnterCode(string code)
        {
            // Validate the partner code exist
            if (_partnerRepository.GetPartnerByCode(code).Result is not Partner partner)
            {
                return Errors.Partner.IncorrectPartnerCode;
            }
            //Create JWT Token
            var token = _jwtTokenGenerator.GeneratePartnerToken(partner);
            return new AuthenticationResult(token);
        }
    }
}
