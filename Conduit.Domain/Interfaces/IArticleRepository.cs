using Conduit.Domain.Entities;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Conduit.Domain.Interfaces
{
    public interface IArticleRepository : IRepository<Article>
    {
        Task<Article> AddTagsToArticle(List<Tag> tags, Article article);
        Task FavoriteArticle(Article article, User currentUser);
        Task<Article> GetArticle(string slug);
        Task<Article> GetArticleWithRelatedDataAsync(string slug);
        Task UnFavoriteArticle(Article favoritedArticle, User currentUser);

        Task<List<Article>> GetFeed(Expression<Func<Article, bool>>? filterCondition, int limit, int offset, [Optional] User currentUser);
        Task<List<Article>> GetFavoriteArticles(User currentUser, int limit, int offset);

    }
}
