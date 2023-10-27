using Coms.Application.Services.Authentication;
using Coms.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Coms.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            var authResult = _authenticationService.Login(request.Username, request.Password);
            var authResponse = new AuthenticationResponse(
                authResult.user.Id,
                authResult.user.FullName,
                authResult.Token
            );
            return Ok(authResponse);
        }
    }
}
