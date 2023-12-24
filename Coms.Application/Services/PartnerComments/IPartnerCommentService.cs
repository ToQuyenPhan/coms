using Coms.Application.Services.Common;
using ErrorOr;

namespace Coms.Application.Services.PartnerComments
{
    public interface IPartnerCommentService
    {
        Task<ErrorOr<PagingResult<PartnerCommentResult>>> GetPartnerComments(int contractId, int currentPage, int pageSize);
        Task<ErrorOr<PartnerCommentResult>> AddPartnerComment(int contractId, string content);
    }
}
