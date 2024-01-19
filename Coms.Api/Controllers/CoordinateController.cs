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

        [HttpGet("contract")]
        [SwaggerOperation(Summary = "Get coordinates in contract by SearchText in Coms")]
        public IActionResult GetContract([FromQuery]CoordinateFormRequest request)
        {
            IList<CoordianteResult> result =
                _coordinateService.GetCoordinatesContract(request.Id ,request.SearchText);
            return Ok(result);
            
        }
        
        [HttpGet("contractAnnex")]
        [SwaggerOperation(Summary = "Get coordinates in contractAnnex by SearchText in Coms")]
        public IActionResult GetContractAnnex([FromQuery] CoordinateFormRequest request)
        {
            IList<CoordianteResult> result =
                _coordinateService.GetCoordinatesContractAnnex(request.Id, request.SearchText);
            return Ok(result);

        }

    }
}
