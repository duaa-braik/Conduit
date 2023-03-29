namespace Conduit.Domain.DTOs
{
    public class ArticleDto
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public DateTime DatePublished { get; set; }
        public DateTime LastModified { get; set; }
        public bool Favorited { get; set; }
        public int FavoritesCount { get; set; }
        public List<string> TagList { get; set; } = new List<string>();
        public UserProfileDto UserProfile { get; set; }
    }
}
