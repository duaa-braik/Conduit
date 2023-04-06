namespace Conduit.Domain.DTOs
{
    public class ArticleCreationDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public List<string> TagList { get; set; } = new List<string>();
    }
}
