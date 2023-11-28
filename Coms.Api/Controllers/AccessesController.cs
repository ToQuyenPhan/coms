﻿using Coms.Application.Services.Accesses;
using Coms.Application.Services.Contracts;
using Coms.Contracts.Contracts;
using Coms.Contracts.UserAccess;
using Coms.Domain.Entities;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Staff")]
    public class AccessesController : ApiController
    {
        private readonly IAccessService _accessService;
        public AccessesController(IAccessService accessService)
        {
            _accessService = accessService;
        }

        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add a access in Coms")]
        public IActionResult Add(int contractId, int accessRoleId)
        {
            ErrorOr<AccessResult> result =
                _accessService.AddAccess(contractId,accessRoleId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
        [HttpPost("addViewers")]
        [SwaggerOperation(Summary = "Add a viewers in Coms")]
        public IActionResult AddViewers(AddViewerOrApproverFormRequest request)
        {
            ErrorOr<IList<AccessResult>> results =
                _accessService.AddViewers(request.Users, request.ContractId).Result;
            return results.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
        [HttpPost("addApprovers")]
        [SwaggerOperation(Summary = "Add a approvers in Coms")]
        public IActionResult AddApprovers(AddViewerOrApproverFormRequest request)
        {
            ErrorOr<IList<AccessResult>> results =
                _accessService.AddApproves(request.Users, request.ContractId).Result;
            return results.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

    }
}
