namespace Coms.Application.Services.PartnerComments
{
    public class PartnerCommentResult
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public int? ReplyId { get; set; }

        public int? PartnerReviewId { get; set; }
        public string Long { get; set; }
        public string CreatedAt { get; set; }
    }
}
