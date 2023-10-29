namespace Coms.Domain.Entities
{
    public class Document_Access
    {
        public int? DocumentId { get; set; }
        public virtual Document Document { get; set; }

        public int? AccessId { get; set; }
        public virtual Access Access { get; set; }
    }
}
