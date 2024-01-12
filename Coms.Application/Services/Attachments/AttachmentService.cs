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
        private readonly IContractAnnexAttachmentRepository _contractAnnexAttachmentRepository;
        public AttachmentService(IAttachmentRepository attachmentRepository, IContractAnnexAttachmentRepository contractAnnexAttachmentRepository)
        {
            _attachmentRepository = attachmentRepository;
            _contractAnnexAttachmentRepository = contractAnnexAttachmentRepository;
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

        //get attachment by contract annex id
        public async Task<ErrorOr<PagingResult<ContractAnnexAttachmentResult>>> GetAttachmentsByContractAnnexId(int contractAnnexId, int currentPage, int pageSize)
        {
            IList<ContractAnnexAttachment>? attachments = await _contractAnnexAttachmentRepository.GetAttachmentsByContractAnnexId(contractAnnexId);
            if (attachments is not null)
            {
                int total = attachments.Count;
                List<ContractAnnexAttachmentResult> attachmentResults = attachments.Select(a => new ContractAnnexAttachmentResult
                {
                    Id = a.Id,
                    FileName = a.FileName,
                    FileLink = a.FileLink,
                    UploadDate = a.UploadDate,
                    Description = a.Description,
                    Status = a.Status,
                    ContractAnnexId = a.ContractAnnexId,
                }).OrderByDescending(a => a.UploadDate).ToList();

                if (currentPage > 0 && pageSize > 0)
                {
                    attachmentResults = attachmentResults.Skip((currentPage - 1) * pageSize).Take(pageSize)
                            .ToList();
                }
                return new
                    PagingResult<ContractAnnexAttachmentResult>(attachmentResults, total, currentPage,
                                       pageSize); ;
            }
            else
            {
                return new PagingResult<ContractAnnexAttachmentResult>(new List<ContractAnnexAttachmentResult>(), 0, currentPage,
                                       pageSize);
            }
        }
        //get attachment by liquidation record id
        //public async Task<ErrorOr<PagingResult<AttachmentResult>>> GetAttachmentsByLiquidationeRecordId(int liquidationeRecordId, int currentPage, int pageSize)
        //{
        //    IList<Attachment>? attachments = await _attachmentRepository.GetAttachmentsByLiquidationeRecordId(liquidationeRecordId);
        //    if (attachments is not null)
        //    {
        //        int total = attachments.Count;
        //        List<AttachmentResult> attachmentResults = attachments.Select(a => new AttachmentResult
        //        {
        //            Id = a.Id,
        //            FileName = a.FileName,
        //            FileLink = a.FileLink,
        //            UploadDate = a.UploadDate,
        //            Description = a.Description,
        //            Status = a.Status,
        //            ContractId = a.ContractId,
        //        }).OrderByDescending(a => a.UploadDate).ToList();
        //        if (currentPage > 0 && pageSize > 0)
        //        {
        //            attachmentResults = attachmentResults.Skip((currentPage - 1) * pageSize).Take(pageSize)
        //                    .ToList();
        //        }
        //        return new
        //            PagingResult<AttachmentResult>(attachmentResults, total, currentPage,
        //                                                  pageSize); ;
        //    }
        //    else
        //    {
        //        return new PagingResult<AttachmentResult>(new List<AttachmentResult>(), 0, currentPage,
        //                                                  pageSize);
        //    }
        //}

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
        //delete attachment by contract annex id
        public async Task<ErrorOr<ContractAnnexAttachmentResult>> DeleteContractAnnexAttachment(int id)
        {
            try
            {
                ContractAnnexAttachment? attachment = await _contractAnnexAttachmentRepository.GetAttachment(id);
                if (attachment is not null)
                {
                    attachment.Status = Domain.Enum.AttachmentStatus.Inactive;
                    await _contractAnnexAttachmentRepository.UpdateAttachment(attachment);
                    ContractAnnexAttachmentResult attachmentResult = new()
                    {
                        Id = attachment.Id,
                        FileName = attachment.FileName,
                        FileLink = attachment.FileLink,
                        UploadDate = attachment.UploadDate,
                        Description = attachment.Description,
                        Status = attachment.Status,
                        ContractAnnexId = attachment.ContractAnnexId,
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
        //add attachment by contract annex id
        public async Task<ErrorOr<ContractAnnexAttachmentResult>> AddContractAnnexAttachment(string fileName, string fileLink, DateTime uploadDate, string description, int contractAnnexId)
        {
            try
            {
                ContractAnnexAttachment attachment = new()
                {
                    FileName = fileName,
                    FileLink = fileLink,
                    UploadDate = uploadDate,
                    Description = description,
                    Status = Domain.Enum.AttachmentStatus.Active,
                    ContractAnnexId = contractAnnexId,
                };
                await _contractAnnexAttachmentRepository.AddAttachment(attachment);
                ContractAnnexAttachmentResult attachmentResult = new()
                {
                    Id = attachment.Id,
                    FileName = attachment.FileName,
                    FileLink = attachment.FileLink,
                    UploadDate = attachment.UploadDate,
                    Description = attachment.Description,
                    Status = attachment.Status,
                    ContractAnnexId = attachment.ContractAnnexId,
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
