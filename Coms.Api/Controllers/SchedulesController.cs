using Coms.Application.Services.Schedules;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    public class SchedulesController : ApiController
    {
        private readonly IScheduleService _scheduleService;

        public SchedulesController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get top schedule of an user in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult GetTopSchdule()
        {
            ErrorOr<ScheduleResult> result = _scheduleService.GetTopSchedule(
                int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value)).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPut("dismiss")]
        [SwaggerOperation(Summary = "Dismiss a schedule of an user in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult DismissSchdule([FromQuery] int id)
        {
            ErrorOr<ScheduleResult> result = _scheduleService.InactiveSchedule(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
