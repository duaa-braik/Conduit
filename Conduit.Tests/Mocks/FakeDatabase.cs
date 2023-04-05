using Conduit.Domain.Entities;

namespace Conduit.Tests.Mocks
{
    public class FakeDatabase
    {
        public List<User> users { get; set; }
        public List<Article> articles { get; set; }
        public List<Follow> Follows { get; set; }
        public List<Comment> comments { get; set; }
        public List<UserArticle> Favorites { get; set; }
        public List<Tag> Tags { get; set; }


        public FakeDatabase()
        {
            users = new List<User>()
            {
                new User { UserId = 1, Username = "duaa", Email = "duaa@gmail.com", Bio = "hii", Password = "123"},
                new User { UserId = 2, Username = "braik", Email = "duaa1@gmail.com", Bio = "hii", Password = "123"},
                new User { UserId = 3, Username = "duaa_braik", Email = "duaa2@gmail.com", Bio = "hii", Password = "123"}
            };

            articles = new List<Article>
            {
                new Article { ArticleId = 1, Title = "Intro to computer science", Slug = "Intro-to-computer-science", UserId = 1, User = users[0] },
                new Article { ArticleId = 2, Title = "Intro to computer engineering", Slug = "Intro-to-computer-engineering", UserId = 2, User = users[1] },
            };

            Follows = new List<Follow>
            {
                new Follow { FollowerId = 1, FolloweeId = 2}
            };

            users[0].Followings.Add(Follows[0]);

            comments = new List<Comment>
            {
                new Comment { CommentId = 1, ArticleId = 1, Body = "this article seems interesting", UserId = 1, User = users[0] },
                new Comment { CommentId = 2, ArticleId = 1, Body = "thank u for the valuable information", UserId = 2, User = users[1] },
            };

            articles[0].Comments.Add(comments[0]);
            articles[0].Comments.Add(comments[1]);

            Tags = new List<Tag>
            {
                new Tag { TagId = 1, TagName = "lorem"},
                new Tag { TagId = 2, TagName = "ipsum"},
                new Tag { TagId = 3, TagName = "test"},
            };

            articles[0].Tags.Add(Tags[0]);
            articles[0].Tags.Add(Tags[1]);

            Favorites = new List<UserArticle>
            {
                new UserArticle { ArticleId = 1, UserId = 1 },
                new UserArticle { ArticleId = 1, UserId = 2 },
                new UserArticle { ArticleId = 2, UserId = 1 },
                new UserArticle { ArticleId = 2, UserId = 2 },
            };

            articles[0].Favorites.Add(Favorites[0]);
            articles[0].Favorites.Add(Favorites[1]);
            articles[1].Favorites.Add(Favorites[0]);
            articles[1].Favorites.Add(Favorites[1]);

        }
    }
}
