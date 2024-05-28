namespace Conduit.Domain.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }

        public int ArticleId { get; set; }
        public Article Article { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
