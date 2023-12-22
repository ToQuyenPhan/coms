using Coms.Application.Services.Common;
using Coms.Application.Services.Contracts;
using ErrorOr;

namespace Coms.Application.Services.Attachments
{
    public interface IAttachmentService
    {
        Task<ErrorOr<PagingResult<AttachmentResult>>> GetAttachmentsByContractId(int contractId, int currentPage, int pageSize);
        Task<ErrorOr<AttachmentResult>> DeleteAttachment(int id);
        Task<ErrorOr<AttachmentResult>> AddAttachment(string fileName, string fileLink, DateTime uploadDate, string description, int contractId);
    }
}
