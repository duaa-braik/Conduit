using System.ComponentModel.DataAnnotations.Schema;

namespace Conduit.Domain.Entities
{
    public class Article
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<UserArticle> Favorites { get; set; }

        public DateTime DatePublished { get; set; }
        public DateTime LastModified { get; set; }

        public Article()
        {
            Comments = new List<Comment>();
            Tags = new List<Tag>();
            Favorites = new List<UserArticle>();
        }

    }
}
