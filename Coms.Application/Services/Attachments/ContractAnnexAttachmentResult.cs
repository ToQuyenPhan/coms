using Coms.Domain.Enum;

namespace Coms.Application.Services.Attachments
{
    public class ContractAnnexAttachmentResult
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? FileLink { get; set; }
        public DateTime UploadDate { get; set; }
        public string? Description { get; set; }
        public AttachmentStatus Status { get; set; }
        public int ContractAnnexId { get; set; }

    }
}
