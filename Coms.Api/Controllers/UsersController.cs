using Coms.Application.Services.Common;
using Coms.Application.Services.Users;
using Coms.Contracts.Common.Paging;
using Coms.Contracts.Users;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    public class UsersController : ApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all users in Coms")]
        //[Authorize(Roles = "Staff")]
        public IActionResult GetUsers([FromQuery] UserFilterRequest request)
        {
            ErrorOr<PagingResult<UserResult>> results = _userService.GetUsers(request.CurrentPage, request.PageSize).Result;
            return results.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("getManagers")]
        [SwaggerOperation(Summary = "Get all Manager in Coms")]
        [Authorize(Roles = "Staff")]
        public IActionResult GetManagers()
        {
            ErrorOr<IList<UserResult>> result = _userService.GetManagers().Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("getStaffs")]
        [SwaggerOperation(Summary = "Get all Staff in Coms")]
        [Authorize(Roles = "Staff")]
        public IActionResult GetStaffs()
        {
            ErrorOr<IList<UserResult>> result = _userService.GetStaffs(int.Parse(this.User.Claims.First(i =>
                i.Type == ClaimTypes.NameIdentifier).Value)).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("current-user")]
        [SwaggerOperation(Summary = "Get current user in Coms")]
        public IActionResult GetCurrentUser()
        {
            ErrorOr<UserResult> result = _userService.GetUser(int.Parse(this.User.Claims.First(i => 
                i.Type == ClaimTypes.NameIdentifier).Value)).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
        
        [HttpGet("id")]
        [SwaggerOperation(Summary = "Get user by id in Coms")]
        public IActionResult GetUserById([FromQuery] int id)
        {
            ErrorOr<UserResult> result = _userService.GetUser(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
