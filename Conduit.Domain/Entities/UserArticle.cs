namespace Conduit.Domain.Entities
{
    public class UserArticle
    {
        public int ArticleId { get; set; }
        public Article Article { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
