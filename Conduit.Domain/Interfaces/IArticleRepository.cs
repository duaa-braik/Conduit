using Conduit.Domain.Entities;

namespace Conduit.Domain.Interfaces
{
    public interface IArticleRepository : IRepository<Article>
    {
        Task<Article> AddTagsToArticle(List<Tag> tags, Article article);
        Task<Article> GetArticleAsync(string slug);
    }
}
