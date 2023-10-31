using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    public class UsersController : ApiController
    {
        [HttpGet]
        public IActionResult ListUsers()
        {
            return Ok(Array.Empty<string>());
        }
    }
}
