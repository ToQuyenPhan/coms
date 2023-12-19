using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Application.Services.Contracts;
using Coms.Domain.Entities;
using ErrorOr;

namespace Coms.Application.Services.Attachments
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
            IList<Attachment>? attachments = await _attachmentRepository.GetAttachmentsByContractId(contractId);
            if (attachments is not null)
            {
                int total = attachments.Count;
                List<AttachmentResult> attachmentResults = attachments.Select(a => new AttachmentResult
                {
                    Id = a.Id,
                    FileName = a.FileName,
                    FileLink = a.FileLink,
                    UploadDate = a.UploadDate,
                    Description = a.Description,
                    Status = a.Status,
                    ContractId = a.ContractId,
                }).OrderByDescending(a => a.UploadDate).ToList();
                if (currentPage > 0 && pageSize > 0)
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

        public async Task<ErrorOr<AttachmentResult>> DeleteAttachment(int id)
        {
            try
            {
                Attachment? attachment = await _attachmentRepository.GetAttachment(id);
                if (attachment is not null)
                {
                    attachment.Status = Domain.Enum.AttachmentStatus.Inactive;
                    await _attachmentRepository.UpdateAttachment(attachment);
                    AttachmentResult attachmentResult = new()
                    {
                        Id = attachment.Id,
                        FileName = attachment.FileName,
                        FileLink = attachment.FileLink,
                        UploadDate = attachment.UploadDate,
                        Description = attachment.Description,
                        Status = attachment.Status,
                        ContractId = attachment.ContractId,
                    };
                    return attachmentResult;
                }
                else
                {
                    return Error.NotFound("404", "Attachment is not found!");
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        //add new attachment
        public async Task<ErrorOr<AttachmentResult>> AddAttachment(string fileName, string fileLink, DateTime uploadDate, string description, int contractId)
        {
            try
            {
                Attachment attachment = new()
                {
                    FileName = fileName,
                    FileLink = fileLink,
                    UploadDate = uploadDate,
                    Description = description,
                    Status = Domain.Enum.AttachmentStatus.Active,
                    ContractId = contractId,
                };
                await _attachmentRepository.AddAttachment(attachment);
                AttachmentResult attachmentResult = new()
                {
                    Id = attachment.Id,
                    FileName = attachment.FileName,
                    FileLink = attachment.FileLink,
                    UploadDate = attachment.UploadDate,
                    Description = attachment.Description,
                    Status = attachment.Status,
                    ContractId = attachment.ContractId,
                };
                return attachmentResult;
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }
    }
}
