﻿using Coms.Application.Services.Common;
using Coms.Application.Services.Services;
using Coms.Contracts.Services;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    public class ServicesController : ApiController
    {
        private readonly IServiceService _serviceService;

        public ServicesController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get active services with filter in Coms")]
        [Authorize(Roles = "Sale Manager, Manager")]
        public IActionResult GetActiveServicesWithFilter([FromQuery] ServiceFilterRequest request)
        {
            ErrorOr<PagingResult<ServiceResult>> result = _serviceService.GetActiveServicesWithFilter(request.ContractCategoryId, 
                    request.ServiceName, request.CurrentPage, request.PageSize).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("active")]
        [SwaggerOperation(Summary = "Get active services in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult GetActiveServices(int? contractCategoryId)
        {
            ErrorOr<IList<ServiceResult>> result = _serviceService.GetActiveServices(contractCategoryId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("gets")]
        [SwaggerOperation(Summary = "Get services by serviceName in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult GetServicesByServiceName([FromQuery]string? serviceName)
        {
            ErrorOr<IList<ServiceResult>> result = _serviceService.GetServicesByName(serviceName).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("get")]
        [SwaggerOperation(Summary = "Get service by id in Coms")]
        [Authorize(Roles = "Staff")]
        public IActionResult GetServiceByServiceId([FromQuery] int serviceId)
        {
            ErrorOr<ServiceResult> result = _serviceService.GetService(serviceId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [Authorize(Roles = "Sale Manager")]
        [HttpPost]
        [SwaggerOperation(Summary = "Add a service in Coms")]
        public IActionResult Add([FromBody]ServiceFormRequest request)
        {
            ErrorOr<ServiceResult> result =
                _serviceService.AddService(request.ServiceName,request.Description, request.Price, request.ContractCategoryId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Update a service in Coms")]
        [Authorize(Roles = "Sale Manager")]
        public IActionResult Update([FromQuery] int id, [FromBody]ServiceFormRequest request)
        {
            ErrorOr<ServiceResult> result = _serviceService.UpdateService(id,request.ServiceName,request.Description,request.Price,
                    request.ContractCategoryId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "Delete a service by serviceId in Coms")]
        [Authorize(Roles = "Sale Manager")]
        public IActionResult Delete([FromQuery] int id)
        {
            ErrorOr<ServiceResult> result = _serviceService.DeleteService(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("getsbyPartnerId")]
        [SwaggerOperation(Summary = "Get services by partnerId in Coms")]
        [Authorize(Roles = "Staff, Manager, Sale Manager")]
        public IActionResult GetServicesByPartnerId([FromQuery] GetSetvicesByPartnerIdRequest request)
        {
            ErrorOr<PagingResult<ServiceResult>> result = _serviceService.GetServicesByPartnerID(request.PartnerId, request.CurrentPage, request.PageSize).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
