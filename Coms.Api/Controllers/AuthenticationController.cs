using Coms.Application.Services.Authentication;
using Coms.Contracts.Authentication;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Coms.Api.Controllers
{
    [Route("auth")]
    [AllowAnonymous]
    public class AuthenticationController : ApiController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        [SwaggerOperation(Summary = "Log in for Coms")]
        public IActionResult Login(LoginRequest request)
        {
            ErrorOr<AuthenticationResult> result =
                _authenticationService.Login(request.Username, request.Password);
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
