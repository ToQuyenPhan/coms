using Coms.Application.Services.Templates;
using Coms.Application.Services.UserAccesses;
using Coms.Application.Services.Users;
using Coms.Contracts.Templates;
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

        [HttpGet("gets")]
        [SwaggerOperation(Summary = "Get all users in Coms")]
        [Authorize(Roles = "Staff")]
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
        
        //add get user by id
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

        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add a user in Coms")]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(UserFormRequest request)
        {
            ErrorOr<UserResult> result =
                _userService.AddUser( request.FullName,request.Username,request.Email, request.Password,request.Dob,request.Image,request.RoleId,request.Status).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Edit a user in Coms")]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(UserFormRequest request, [FromQuery] int userId)
        {
            ErrorOr<UserResult> result =
                _userService.UpdateUser(request.FullName, request.Username, request.Email, request.Password, request.Dob, request.Image, request.RoleId, request.Status, userId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "Delete a user in Coms")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            ErrorOr<UserResult> result = _userService.DeleteUser(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
