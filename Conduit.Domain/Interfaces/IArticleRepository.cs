using Conduit.Domain.Entities;

namespace Conduit.Domain.Interfaces
{
    public interface IArticleRepository : IRepository<Article>
    {
        Task<Article> AddTagsToArticle(List<Tag> tags, Article article);
        Task FavoriteArticle(Article article, User currentUser);
        Task<Article> GetArticle(string slug);
        Task<Article> GetArticleWithRelatedDataAsync(string slug);
        Task UnFavoriteArticle(Article favoritedArticle, User currentUser);
    }
}
