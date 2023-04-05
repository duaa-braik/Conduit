using Conduit.Domain.Entities;
using Conduit.Domain.Interfaces;
using Moq;
using System.Security.Cryptography;

namespace Conduit.Tests.Mocks
{
    public class ArticleRepositoryMocks : FakeDatabase
    {
        public Mock<IArticleRepository> articleRepositoryMock { get; set; }

        public ArticleRepositoryMocks()
        {
            articleRepositoryMock = new Mock<IArticleRepository>();

            articleRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Article>()))
                .Callback((Article article) => article.ArticleId = RandomNumberGenerator.GetInt32(100));

            articleRepositoryMock.Setup(x => x.AddTagsToArticle(It.IsAny<List<Tag>>(), It.IsAny<Article>()))
                .Callback((List<Tag> tags, Article article) => {
                    var Tags = article.Tags as List<Tag>;
                    Tags.AddRange(tags);
                });

            articleRepositoryMock.Setup(x => x.GetArticle
                (It.IsAny<string>()))
                .ReturnsAsync((Article article) => articles.First(a => a.Slug == article.Slug));

            articleRepositoryMock.Setup(x => x.GetArticleWithRelatedDataAsync
                (It.IsAny<string>()))
                .ReturnsAsync((string slug) =>
                {
                    var article = articles.First(a => a.Slug == slug);
                    article.User = users.First(u => u.UserId == article.UserId);
                    return article;
                });
        }
    }
}
