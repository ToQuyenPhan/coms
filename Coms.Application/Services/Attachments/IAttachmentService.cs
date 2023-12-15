using Coms.Application.Services.Common;
using ErrorOr;

namespace Coms.Application.Services.Contracts
{
    public interface IAttachmentService
    {
        Task<ErrorOr<PagingResult<AttachmentResult>>> GetAttachmentsByContractId(int contractId, int currentPage, int pageSize);
    }
}
