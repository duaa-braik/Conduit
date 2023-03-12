namespace Conduit.Domain.Entities
{
    public class Tag
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
