namespace Coms.Contracts.Comments
{
    public class CommentFormUpdateRequest
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
