using Coms.Application.Services.Comments;
using Coms.Application.Services.Common;
using Coms.Contracts.Comments;
using Coms.Contracts.Common.Paging;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Staff, Manager")]
    public class CommentsController : ApiController
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("all")]
        [SwaggerOperation(Summary = "Get all comments of your contracts in Coms")]
        public IActionResult GetYourContracts([FromQuery] PagingRequest request)
        {
            ErrorOr<PagingResult<CommentResult>> result =
                _commentService.GetAllComments(int.Parse(this.User.Claims.First(i => i.Type ==
                ClaimTypes.NameIdentifier).Value), request.CurrentPage, request.PageSize).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get details of a comment in Coms")]
        public IActionResult GetCommentDetails([FromQuery] int id)
        {
            ErrorOr<CommentDetailResult> result = _commentService.GetCommentDetail(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpGet("contract")]
        [SwaggerOperation(Summary = "Get all comments of a contract in Coms")]
        public IActionResult GetContractComments([FromQuery] int contractId, [FromQuery] PagingRequest request)
        {
            ErrorOr<PagingResult<CommentResult>> result =
                _commentService.GetContractComments(contractId, request.CurrentPage, request.PageSize).Result;
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

        [HttpGet("contract-partner")]
        [SwaggerOperation(Summary = "Get all partner comments of a contract in Coms")]
        public IActionResult GetContractPartnerComments([FromQuery] int contractId, [FromQuery] PagingRequest request)
        {
            ErrorOr<PagingResult<CommentResult>> result =
                _commentService.GetContractComments(contractId, request.CurrentPage, request.PageSize).Result;
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Leave a comment in Coms")]
        public IActionResult LeaveComment([FromBody] CommentFormRequest request)
        {
            ErrorOr<CommentResult> result = _commentService.LeaveComment(
                    int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value), 
                    request.ContractId, request.Content, request.ReplyId, request.CommentType).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Edit a comment in Coms")]
        public IActionResult EditComment([FromBody] CommentFormUpdateRequest request)
        {
            ErrorOr<CommentResult> result = _commentService.EditComment(request.Id, request.Content).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "Delete a comment in Coms")]
        public IActionResult DeleteComment([FromQuery] int id)
        {
            ErrorOr<CommentResult> result = _commentService.DeleteComment(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        #region ContractAnnex
        //get all comments of a contract annex
        [HttpGet("annex")]
        [SwaggerOperation(Summary = "Get all comments of a contract annex in Coms")]
        public IActionResult GetContractAnnexComments([FromQuery] int contractAnnexId, [FromQuery] PagingRequest request)
        {
            ErrorOr<PagingResult<CommentResult>> result =
                _commentService.GetContractAnnexComments(contractAnnexId, request.CurrentPage, request.PageSize).Result;
            return result.Match(result => Ok(result), errors => Problem(errors));
        }
        //leave comment of contract annex
        [HttpPost("annex")]
        [SwaggerOperation(Summary = "Leave a comment of a contract annex in Coms")]
        public IActionResult LeaveContractAnnexComment([FromBody] ContractAnnexCommentFormRequest request)
        {
            ErrorOr<CommentResult> result = _commentService.LeaveContractAnnexComment(
                                   int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value), request.ContractAnnexId,
                                                      request.Content, request.ReplyId).Result;
            return result.Match(
                               result => Ok(result),
                                              errors => Problem(errors)
                                                         );
        }

        #endregion
    }
}
