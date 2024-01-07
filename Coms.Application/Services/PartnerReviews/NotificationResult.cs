namespace Coms.Application.Services.PartnerReviews
{
    public class NotificationResult
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Long { get; set; }
        public int? ContractId { get; set; }
        public DateTime? Time { get; set; }
    }
}
