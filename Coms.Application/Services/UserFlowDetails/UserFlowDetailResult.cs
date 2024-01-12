namespace Coms.Application.Services.UserFlowDetails
{
    public class UserFlowDetailResult
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string StatusString { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string UserImage { get; set; } = string.Empty;
        public int? ContractId { get; set; }
        public int? ContractAnnexId { get; set; }
        public int FlowDetailId { get; set; }
        public string FlowRole { get; set; } = string.Empty;
    }
}
