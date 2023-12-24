namespace Coms.Contracts.PartnerComments
{
    public class PartnerCommentFormRequest
    {
        public int ContractId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
