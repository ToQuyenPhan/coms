using Coms.Application.Services.Common;
using ErrorOr;

namespace Coms.Application.Services.Contracts
{
    public interface IAttachmentService
    {
        //add get all attachments by contract id
        Task<ErrorOr<IList<AttachmentResult>>> GetAttachmentsByContractId(int contractId);
    }
}
