namespace Coms.Contracts.Comments
{
    public class CommentFormRequest
    {
        public int ContractId {  get; set; }
        public string Content {  get; set; } = string.Empty;
        public int? ReplyId { get; set; }
    }
}
