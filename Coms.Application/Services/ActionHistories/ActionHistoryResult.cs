namespace Coms.Application.Services.ActionHistories
{
    public class ActionHistoryResult
    {
        public int Id { get; set; }
        public int ActionType { get; set; }
        public string ActionTypeString { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedAtString { get; set; }
        public int? UserId { get; set; }
        public string FullName { get; set; }
        public string UserImage { get; set; }

        public int? ContractId { get; set; }
        public string ContractName { get; set; }
    }
}
