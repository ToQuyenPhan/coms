using Coms.Application.Services.Contracts;
using Coms.Application.Services.Services;
using Coms.Application.Services.Users;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Staff")]
    public class UsersController : ApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("gets")]
        [SwaggerOperation(Summary = "Get all users in Coms")]
        public IActionResult GetUsers()
        {
            ErrorOr<IList<UserResult>> result = _userService.GetUsers().Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
        [HttpGet("getManagers")]
        [SwaggerOperation(Summary = "Get all Manager in Coms")]
        public IActionResult GetManagers()
        {
            ErrorOr<IList<UserResult>> result = _userService.GetManagers().Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

    }
}
