namespace Coms.Application.Services.UserFlowDetails
{
    public class NotificationResult
    {
        public string Title {  get; set; }
        public string Message { get; set; }
        public string Long {  get; set; }
        public int? ContractId {  get; set; }
        public int? ContractAnnexId {  get; set; }
        public DateTime Time { get; set; }
        public string Type {  get; set; }
    }
}
