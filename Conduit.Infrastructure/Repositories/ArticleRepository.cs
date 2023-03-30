using Conduit.Domain.Entities;
using Conduit.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Article> GetArticleAsync(string slug)
        {
            return await context.Article
                .Include(u => u.User)
                .Include(u => u.Tags)
                .FirstAsync(a => a.Slug == slug);
        }
    }
}
