using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using ErrorOr;

namespace Coms.Application.Services.Contracts
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepository _attachmentRepository;
        public AttachmentService(IAttachmentRepository attachmentRepository)
        {
            _attachmentRepository = attachmentRepository;
        }

        public async Task<ErrorOr<PagingResult<AttachmentResult>>> GetAttachmentsByContractId(int contractId, int currentPage,
                int pageSize)
        {
            var attachments = await _attachmentRepository.GetAttachmentsByContractId(contractId);
            if (attachments is not null)
            {
                int total = attachments.Count;
                var attachmentResults = attachments.Select(a => new AttachmentResult
                {
                    Id = a.Id,
                    FileName = a.FileName,
                    FileLink = a.FileLink,
                    UploadDate = a.UploadDate,
                    Description = a.Description,
                    Status = a.Status,
                    ContractId = a.ContractId,
                }).OrderByDescending(a => a.UploadDate).ToList();
                if(currentPage > 0 &&  pageSize > 0)
                {
                    attachmentResults = attachmentResults.Skip((currentPage - 1) * pageSize).Take(pageSize)
                            .ToList();
                }
                return new
                    PagingResult<AttachmentResult>(attachmentResults, total, currentPage,
                    pageSize); ;
            }
            else
            {
                return new PagingResult<AttachmentResult>(new List<AttachmentResult>(), 0, currentPage,
                    pageSize);
            }
        }
    }
}
