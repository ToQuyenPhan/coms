using Coms.Application.Services.Comments;
using Coms.Application.Services.Common;
using Coms.Contracts.Common.Paging;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Coms.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Staff")]
    public class CommentsController : ApiController
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("all")]
        [SwaggerOperation(Summary = "Get all comment of your contracts in Coms")]
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

        [HttpPut("dismiss")]
        [SwaggerOperation(Summary = "Dismiss a comment in Coms")]
        public IActionResult DismissComment([FromQuery] int id)
        {
            ErrorOr<CommentResult> result = _commentService.DismissComment(id).Result;
            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        //get all comment of a contract
        [HttpGet("contract")]
        [SwaggerOperation(Summary = "Get all comment of a contract in Coms")]
        public IActionResult GetContractComments([FromQuery] int contractId, [FromQuery] PagingRequest request)
        {
            ErrorOr<PagingResult<CommentResult>> result =
                _commentService.GetContractComments(contractId, request.CurrentPage, request.PageSize).Result;
            return result.Match(
                               result => Ok(result),
                                              errors => Problem(errors)
                                                         );
        }
    }
}
