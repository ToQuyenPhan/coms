using Coms.Application.Services.SystemSettingService;
using Coms.Contracts.SystemSettings;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    public class SystemSettingsController : ApiController
    {
        private readonly ISystemSettingService _systemSettingService;

        public SystemSettingsController(ISystemSettingService systemSettingService)
        {
            _systemSettingService = systemSettingService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get system settings in Coms")]
        [Authorize(Roles = "Admin")]
        public IActionResult Get()
        {
            ErrorOr<SystemSettingResult> results = _systemSettingService.GetSystemSettings().Result;
            return results.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Update system settings in Coms")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update([FromBody] SystemSettingFormRequest request)
        {
            ErrorOr<SystemSettingResult> results = _systemSettingService.EditSystemSettings(request.CompanyName, request.Address, 
                    request.Phone, request.Hotline, request.TaxCode, request.Email, request.BankAccount, request.BankAccountNumber, 
                    request.BankName, request.AppPassword).Result;
            return results.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
