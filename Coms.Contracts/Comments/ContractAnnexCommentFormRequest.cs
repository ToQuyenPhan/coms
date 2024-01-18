namespace Coms.Contracts.Comments
{
    public class ContractAnnexCommentFormRequest
    {
        public int ContractAnnexId {  get; set; }
        public string Content {  get; set; } = string.Empty;
        public int? ReplyId { get; set; }
        public int CommentType { get; set; }
    }
}
