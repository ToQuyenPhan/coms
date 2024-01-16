namespace Coms.Contracts.Attachments
{
    public class AddContractAnnexAttachmentRequest
    {
        public string? FileName { get; set; }
        public string? FileLink { get; set; }
        public DateTime UploadDate { get; set; }
        public string? Description { get; set; }
        public int ContractAnnexId { get; set; }
    }
}
