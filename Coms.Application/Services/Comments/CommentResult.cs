namespace Coms.Application.Services.Comments
{
    public class CommentResult
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public int? ReplyId { get; set; }
        public int Status { get; set; }
        public string StatusString { get; set; }

        public int? ActionHistoryId { get; set; }
        public string Long { get; set; }
        public string CreatedAt { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string AccessRole { get; set; }
        public string UserImage {  get; set; }
        public string CommentType { get; set; }
    }
}
