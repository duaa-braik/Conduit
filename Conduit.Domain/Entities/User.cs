namespace Conduit.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Username{ get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public IEnumerable<Follow> Followers { get; set; }
        public IEnumerable<Follow> Followings { get; set; }
        
        public IEnumerable<Article> Articles { get; set; }
        public IEnumerable<UserArticle> Favorites { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
    }
}
