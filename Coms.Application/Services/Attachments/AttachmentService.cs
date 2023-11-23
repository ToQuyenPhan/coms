using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;
using LinqKit;

namespace Coms.Application.Services.Contracts
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepository _attachmentRepository;
        public AttachmentService(IAttachmentRepository attachmentRepository)
        {
            _attachmentRepository = attachmentRepository;
        }
        //add get all attachments by contract id
        public async Task<ErrorOr<IList<AttachmentResult>>> GetAttachmentsByContractId(int contractId)
        {
            var attachments = await _attachmentRepository.GetAttachmentsByContractId(contractId);
            var attachmentResults = attachments.Select(a => new AttachmentResult
            {
                Id = a.Id,
                FileName = a.FileName,
                FileLink = a.FileLink,
                UploadDate = a.UploadDate,
                Description = a.Description,
                Status = a.Status,
                ContractId = a.ContractId,
            }).ToList();
            return attachmentResults;
        }


    }
}
