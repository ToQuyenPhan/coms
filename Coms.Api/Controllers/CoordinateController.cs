using Coms.Application.Services.Accesses;
using Coms.Application.Services.Coordinates;
using Coms.Contracts.Coordinates;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [AllowAnonymous]
    public class CoordinateController : ApiController
    {
        private readonly ICoordinateService _coordinateService;
        public CoordinateController(ICoordinateService coordinateService)
        {
            _coordinateService = coordinateService;
        }

        [HttpGet("get")]
        [SwaggerOperation(Summary = "Get coordinates by SearchText in Coms")]
        public IActionResult Add([FromQuery]CoordinateFormRequest request)
        {
            IList<CoordianteResult> result =
                _coordinateService.GetCoordinates(request.ContractId ,request.SearchText);
            return Ok(result);
            
        }

    }
}
