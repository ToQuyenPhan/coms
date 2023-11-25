﻿using Coms.Application.Services.Accesses;
using Coms.Application.Services.Common;
using Coms.Application.Services.Contracts;
using Coms.Application.Services.Services;
using Coms.Contracts.Contracts;
using Coms.Contracts.Services;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Staff")]
    public class ServicesController : Controller
    {
        private readonly IServiceService _serviceService;

        public ServicesController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet("gets")]
        [SwaggerOperation(Summary = "Get services by serviceName in Coms")]
        public IActionResult GetServicesByServiceName([FromQuery]string? serviceName)
        {
            ErrorOr<IList<ServiceResult>> result = _serviceService.GetServicesByName(serviceName).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem()
            );
        }

        [HttpGet("get")]
        [SwaggerOperation(Summary = "Get services by serviceName in Coms")]
        public IActionResult GetServiceByServiceId([FromQuery] int serviceId)
        {
            ErrorOr<ServiceResult> result = _serviceService.GetService(serviceId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem()
            );
        }

        [Authorize(Roles = "Sale Manager")]
        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add a service in Coms")]
        public IActionResult Add([FromBody]ServiceFormRequest request)
        {
            ErrorOr<ServiceResult> result =
                _serviceService.AddService(request.ServiceName,request.Description, request.Price ).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem()
            );
        }

        [HttpPut("update")]
        [SwaggerOperation(Summary = "Update a service in Coms")]
        public IActionResult Update([FromQuery] int serviceId, [FromBody]ServiceFormRequest request)
        {
            ErrorOr<ServiceResult> result = _serviceService.UpdateService(serviceId,request.ServiceName,request.Description,request.Price).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem()
            );
        }

        [HttpDelete("delete")]
        [SwaggerOperation(Summary = "Delete a service by serviceId in Coms")]
        public IActionResult Delete([FromQuery] int serviceId)
        {
            ErrorOr<ServiceResult> result = _serviceService.DeleteService(serviceId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem()
            );
        }
    }
}