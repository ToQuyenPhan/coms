using Coms.Domain.Enum;

namespace Coms.Application.Services.Contracts
{
    public class AttachmentResult
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileLink { get; set; }
        public DateTime UploadDate { get; set; }
        public string Description { get; set; }
        public AttachmentStatus Status { get; set; }
        public int ContractId { get; set; }

    }
}
