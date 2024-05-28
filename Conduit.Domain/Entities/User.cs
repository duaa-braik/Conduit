﻿namespace Conduit.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Username{ get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Bio { get; set; }

        public ICollection<Follow> Followers { get; set; }
        public ICollection<Follow> Followings { get; set; }
        
        public ICollection<Article> Articles { get; set; }
        public ICollection<UserArticle> Favorites { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public User()
        {
            Followers = new List<Follow>();
            Followings = new List<Follow>();
            Articles = new List<Article>();
            Favorites = new List<UserArticle>();
            Comments = new List<Comment>();
        }
    }
}
