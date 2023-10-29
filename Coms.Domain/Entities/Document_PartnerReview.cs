namespace Coms.Domain.Entities
{
    public class Document_PartnerReview
    {
        public int? DocumentId { get; set; }
        public virtual Document Document { get; set; }

        public int? PartnerReviewId { get; set; }
        public virtual PartnerReview PartnerReview { get; set; }
    }
}
