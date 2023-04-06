using Conduit.Domain.Entities;
using Conduit.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Conduit.Infrastructure.Repositories
{
    public class ArticleRepository : GenericRepository<Article>, IArticleRepository
    {
        public ArticleRepository(ConduitDbContext context) : base(context) { }

        public async Task<Article> AddTagsToArticle(List<Tag> tags, Article article)
        {
            var Tags = article.Tags as List<Tag>;
            Tags.AddRange(tags);
            await context.SaveChangesAsync();
            return article;
        }

        public async Task<Article> GetArticleWithRelatedDataAsync(string slug)
        {
            return await GetArticlesQuery().FirstAsync(a => a.Slug == slug);
        }

        public override async Task<Article> UpdateAsync(Article article)
        {
            context.Article.Attach(article);
            context.Entry(article).Property("Title").IsModified = true;
            await context.SaveChangesAsync();
            return article;
        }

        public async Task<Article> GetArticle(string slug)
        {
            return await context.Article.Include(a => a.User).FirstAsync(a => a.Slug == slug);
        }

        public async Task FavoriteArticle(Article article, User currentUser)
        {
            article.Favorites.Add(new UserArticle { User = currentUser, Article = article });
            await context.SaveChangesAsync();
        }

        public async Task UnFavoriteArticle(Article favoritedArticle, User currentUser)
        {
            var favorite = favoritedArticle.Favorites.FirstOrDefault(f => f.User == currentUser);
            favoritedArticle.Favorites.Remove(favorite);
            await context.SaveChangesAsync();
        }

        public async Task<List<Article>> GetFeed(Expression<Func<Article, bool>>? filter, int limit, int offset, [Optional] User currentUser)
        {
            var articles = GetArticlesQuery();

            if(filter != null)
            {
                articles = articles.Where(filter);
            }
            
            if (currentUser != null)
            {
                articles = FilterArticlesByFollowings(currentUser, articles);
            }

            return await TakeOnePage(articles, limit, offset);
        }

        public async Task<List<Article>> GetFavoriteArticles(User currentUser, int limit, int offset)
        {
            var articles = GetArticlesQuery().Where(a => a.Favorites.Any(f => f.User == currentUser));

            return await TakeOnePage(articles, limit, offset);
        }

        private static async Task<List<Article>> TakeOnePage(IQueryable<Article> articles, int limit, int offset)
        {
            limit = limit == 0 ? 20 : limit;
            offset = offset == 0 ? 1 : offset;
            return await articles
                    .OrderByDescending(a => a.DatePublished)
                    .Skip(limit * (offset - 1))
                    .Take(limit).ToListAsync();
        }

        private static IQueryable<Article> FilterArticlesByFollowings(User currentUser, IQueryable<Article> articles)
        {
            return articles.Where(a => currentUser.Followings.Select(f => f.FolloweeId).Contains(a.UserId));
        }

        private IQueryable<Article> GetArticlesQuery()
        {
            return context.Article
                    .Include(u => u.User)
                    .Include(u => u.Tags)
                    .Include(u => u.Favorites);
        }
    }
}
