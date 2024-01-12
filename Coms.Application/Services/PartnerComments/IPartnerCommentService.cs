using Coms.Application.Services.Common;
using ErrorOr;

namespace Coms.Application.Services.PartnerComments
{
    public interface IPartnerCommentService
    {
        Task<ErrorOr<PartnerCommentResult>> GetPartnerComment(int contractId);
        Task<ErrorOr<PartnerCommentResult>> AddPartnerComment(int contractId, string content);
        Task<ErrorOr<PartnerCommentResult>> GetPartnerCommentByContractAnnexId(int contractAnnexId);
    }
}
