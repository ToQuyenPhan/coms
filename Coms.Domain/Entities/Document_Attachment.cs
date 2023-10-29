namespace Coms.Domain.Entities
{
    public class Document_Attachment
    {
        public int? DocumentId { get; set; }
        public virtual Document Document { get; set; }

        public int? AttachmentId { get; set; }
        public virtual Attachment Attachment { get; set; }
    }
}
