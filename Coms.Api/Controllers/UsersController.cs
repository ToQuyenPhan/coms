using Coms.Application.Services.Common;
using Coms.Application.Services.Users;
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
        [Authorize(Roles = "Admin")]
        public IActionResult GetUsers([FromQuery] UserFilterRequest request)
        {
            ErrorOr<PagingResult<UserResult>> results = _userService.GetUsers(request.Fullname, request.Email, request.RoleId, 
                    request.Status, request.CurrentPage, request.PageSize).Result;
            return results.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("id")]
        [SwaggerOperation(Summary = "Get user by id in Coms")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetUserById([FromQuery] int id)
        {
            ErrorOr<UserResult> result = _userService.GetUser(id).Result;
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

        [HttpPost]
        [SwaggerOperation(Summary = "Add an user in Coms")]
        [Authorize(Roles = "Admin")]
        public IActionResult ActiveUser([FromBody] UserFormRequest request)
        {
            ErrorOr<UserResult> result = _userService.AddUser(request.FullName, request.Username, request.Dob, request.Image, 
                    request.Password, request.RoleId, request.Email, request.Position, request.Phone).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Edit an user in Coms")]
        public IActionResult InactiveUser([FromQuery] int id, [FromBody] UserFormRequest request)
        {
            ErrorOr<UserResult> result = _userService.EditUser(id, request.FullName, request.Username, request.Dob, request.Image,
                    request.Password, request.RoleId, request.Email, request.Position, request.Phone).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPut("inactive")]
        [SwaggerOperation(Summary = "Inactive an user in Coms")]
        [Authorize(Roles = "Admin")]
        public IActionResult InactiveUser([FromQuery] int id)
        {
            ErrorOr<UserResult> result = _userService.InactiveUser(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPut("active")]
        [SwaggerOperation(Summary = "Active an user in Coms")]
        [Authorize(Roles = "Admin")]
        public IActionResult ActiveUser([FromQuery] int id)
        {
            ErrorOr<UserResult> result = _userService.ActiveUser(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
