﻿using Conduit.Domain.Entities;
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

        public async Task<Article> GetArticleWithRelatedDataAsync(string slug)
        {
            return await context.Article
                .Include(u => u.User)
                .Include(u => u.Tags)
                .Include(u => u.Favorites)
                .FirstAsync(a => a.Slug == slug);
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
    }
}
