namespace Conduit.Domain.Entities
{
    public class Article
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public IEnumerable<UserArticle> Favorites { get; set; }

        public DateTime DatePublished { get; set; }
        public DateTime LastModified { get; set; }

    }
}
