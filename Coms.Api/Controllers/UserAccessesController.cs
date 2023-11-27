using Coms.Application.Services.Accesses;
using Coms.Application.Services.UserAccesses;
using Coms.Application.Services.Users;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Staff")]
    public class UserAccessesController : ApiController
    {
        private readonly IUserAccessService _userAccessService;

        public UserAccessesController(IUserAccessService userAccessService)
        {
            _userAccessService = userAccessService;
        }
        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add a user access in Coms")]
        public IActionResult Add(int contractId,int userId, int accessRoleId)
        {
            ErrorOr<UserAccessResult> result =
                _userAccessService.AddUserAccess(contractId,userId, accessRoleId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
