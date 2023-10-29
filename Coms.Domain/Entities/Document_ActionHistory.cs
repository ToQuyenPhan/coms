namespace Coms.Domain.Entities
{
    public class Document_ActionHistory
    {
        public int? ActionHistoryId { get; set; }
        public virtual ActionHistory ActionHistory { get; set; }

        public int? DocumentId { get; set; }
        public virtual Document Document { get; set; }
    }
}
