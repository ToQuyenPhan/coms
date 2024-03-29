﻿using Coms.Application.Services.Common;
using Coms.Application.Services.Contracts;
using Coms.Contracts.Contracts;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    public class ContractsController : ApiController
    {
        private readonly IContractService _contractService;

        public ContractsController(IContractService contractService)
        {
            _contractService = contractService;
        }

        [HttpGet("yours")]
        [SwaggerOperation(Summary = "Get your contracts in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult GetYourContracts([FromQuery] YourContractsFilterRequest request)
        {
            ErrorOr<PagingResult<ContractResult>> result = _contractService.GetYourContracts(
                int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value),
                request.ContractName, request.Code, request.Version, request.Status, request.IsYours, request.CurrentPage,
                request.PageSize).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("author")]
        [SwaggerOperation(Summary = "Check contract author in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult IsAuthor([FromQuery] int contractId)
        {
            ErrorOr<AuthorResult> result = _contractService.IsAuthor(
                int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value), contractId).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "Delete a contract in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult Delete([FromQuery] int id)
        {
            ErrorOr<ContractResult> result = _contractService.DeleteContract(
                    int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value), id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("general-report")]
        [SwaggerOperation(Summary = "Get contract general report in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult GetGeneralReport()
        {
            ErrorOr<IList<GeneralReportResult>> result = _contractService.GetGeneralReport(
                int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value)).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("id")]
        [SwaggerOperation(Summary = "Get contract by id in Coms")]
        public IActionResult GetContractById([FromQuery] int id)
        {
            ErrorOr<ContractResult> result = _contractService.GetContract(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("partner-service")]
        [SwaggerOperation(Summary = "Get contract partner and service in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult GetContractPartnerAndService([FromQuery] int id)
        {
            ErrorOr<ContractResult> result = _contractService.GetPartnerAndService(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Add a contract in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult AddContract([FromBody] ContractFormRequest request)
        {
            ErrorOr<int> result = _contractService.AddContract(request.Name, request.Value, (int)request.ContractCategoryId,
                        request.ServiceId, request.EffectiveDate, request.Status,
                        int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value),
                        request.PartnerId, (int)request.TemplateType, request.ApproveDate, request.SignDate).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPost("upload")]
        [SwaggerOperation(Summary = "Upload a contract in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult UploadContract([FromQuery] int id)
        {
            ErrorOr<string> result = _contractService.UploadContract(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPost("preview")]
        [SwaggerOperation(Summary = "Upload a contract in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult PreviewContract([FromBody] ContractPreviewRequest request)
        {
            ErrorOr<MemoryStream> result = _contractService.PreviewContract(request.Name, request.Value,
                    request.ContractCategoryId, request.TemplateType).Result;
            return result.Match(
                result => new FileStreamResult(result, "application/pdf"),
                errors => Problem(errors)
            );
        }

        [HttpGet("manager")]
        [SwaggerOperation(Summary = "Get contract list for manager in Coms")]
        [Authorize(Roles = "Manager")]
        public IActionResult GetManagerContracts([FromQuery] YourContractsFilterRequest request)
        {
            ErrorOr<PagingResult<ContractResult>> result = _contractService.GetManagerContracts(
                int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value),
                request.ContractName, request.Code, request.PartnerName, request.Version, request.Status, request.CurrentPage,
                request.PageSize).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("partner")]
        [SwaggerOperation(Summary = "Get contract list for partner in Coms")]
        public IActionResult GetPartnerContracts([FromQuery] PartnerContractFilterRequest request)
        {
            ErrorOr<PagingResult<ContractResult>> result = _contractService.GetContractForPartner(
                int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value),
                request.ContractName, request.Code, request.Version, request.DocumentStatus, request.IsApproved, request.CurrentPage,
                request.PageSize).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Edit a contract in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult EditContract([FromQuery] int contractId, [FromBody] ContractFormRequest request)
        {
            ErrorOr<int> result = _contractService.EditContract(contractId, request.Name, request.Value, request.ServiceId,
                    request.EffectiveDate, request.Status,
                    int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value),
                    request.PartnerId, request.ApproveDate, request.SignDate).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPut("approveOrReject")]
        [SwaggerOperation(Summary = "Approve or reject a contract by manager in Coms")]
        [Authorize(Roles = "Manager")]
        public IActionResult ApproveContract([FromQuery] int id, bool isApproved)
        {
            ErrorOr<ContractResult> result = _contractService
                    .ApproveContract(id, int.Parse(this.User.Claims.First(i =>
                    i.Type == ClaimTypes.NameIdentifier).Value), isApproved).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPut("reject")]
        [SwaggerOperation(Summary = "Reject a contract by a partner in Coms")]
        public IActionResult RejectContract([FromQuery] int id, bool isApproved)
        {
            ErrorOr<ContractResult> result = _contractService.RejectContract(id, isApproved).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }


        [HttpGet("manager/sign")]
        [SwaggerOperation(Summary = "Get contract list for manager to sign in Coms")]
        [Authorize(Roles = "Manager")]
        public IActionResult GetManagerContractsForSign([FromQuery] YourContractsFilterRequest request)
        {
            ErrorOr<PagingResult<ContractResult>> result = _contractService.GetManagerContractsForSign(
                int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value),
                request.ContractName, request.Code, request.Status, request.CurrentPage,
                request.PageSize).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("statistics")]
        [SwaggerOperation(Summary = "Get contracts by Service or Partner in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult GetContractsByServiceIdOrPartnerId([FromQuery] ContractStatisticFilterRequest request)
        {
            ErrorOr<PagingResult<ContractResult>> result = _contractService.GetContractsByServiceOrPartner(
                request.ContractName, request.Code, request.Status, request.ServiceId, request.PartnerId, request.StartDate, request.EndDate
                , request.CurrentPage, request.PageSize).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("general-reports")]
        [SwaggerOperation(Summary = "Get all contract general reports in Coms")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult GetGeneralReports()
        {
            ErrorOr<IList<GeneralReportResult>> result = _contractService.GetGeneralReport().Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        //get list contract approved or rejected of user
        [HttpGet("user")]
        [SwaggerOperation(Summary = "Get list contract approved or rejected of user in Coms")]
        [Authorize(Roles = "Manager")]
        public IActionResult GetListContractApprovedOrRejectedOfUser([FromQuery] YourContractsFilterRequest request)
        {
            ErrorOr<PagingResult<ContractResult>> result = _contractService.GetListContractApprovedOrRejectedOfUser(
                               int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value), request.Status, request.CurrentPage, request.PageSize).Result;
            return result.Match(
                               result => Ok(result),
                                              errors => Problem(errors)
                                                         );
        }
    }
}
