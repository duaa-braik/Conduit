using AutoMapper;
using Conduit.Application.Interfaces;
using Conduit.Domain.DTOs;
using Conduit.Domain.Entities;
using Conduit.Domain.Exceptions;
using Conduit.Domain.Interfaces;
using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Conduit.Application.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository articleRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly ITagRepository tagRepository;
        private readonly IProfileService profileService;
        private readonly ICommentRepository commentRepository;
        private bool withRelatedData;

        public ArticleService
            (IArticleRepository articleRepository, IUserRepository userRepository,
            IMapper mapper, ITagRepository tagRepository, IProfileService profileService, ICommentRepository commentRepository)
        {
            this.articleRepository = articleRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.tagRepository = tagRepository;
            this.profileService = profileService;
            this.commentRepository = commentRepository;
        }

        public async Task<ArticleDto> AddArticle(ArticleCreationDto articleDetails, string Username)
        {
            User Publisher = await userRepository.GetUserByUsername(Username);

            Article article = mapper.Map<Article>(articleDetails);

            article.User = Publisher;

            await articleRepository.CreateAsync(article);

            await AddTags(articleDetails.TagList, article);

            ArticleDto articleDto = mapper.Map<ArticleDto>(article);

            return articleDto;
        }

        public async Task<ArticleDto> GetArticle(string slug)
        {
            withRelatedData = true;
            Article article = await GetArticle(slug, withRelatedData);

            ArticleDto articleDto = mapper.Map<ArticleDto>(article);

            return articleDto;
        }

        public async Task<ArticleDto> GetArticle(string slug, string CurrentUserName)
        {
            withRelatedData = true;
            Article article = await GetArticle(slug, withRelatedData);

            User currentUser = await userRepository.GetUserWithFollowings(CurrentUserName);

            ArticleDto articleDto = mapper.Map<ArticleDto>(article);

            MapFollowAndFavoriteStatus(article, currentUser, articleDto);

            return articleDto;
        }

        public async Task<ArticleDto> UpdateArticle(string slug, ArticleUpdateDto articleUpdates, string CurrentUserName)
        {
            withRelatedData = true;

            Article article = await GetArticle(slug, withRelatedData);

            checkUserPermission(CurrentUserName, article.User.Username);

            string updatedTitle = articleUpdates.Title;

            article.Title = updatedTitle;
            article.LastModified = DateTime.UtcNow;
            article.Slug = updatedTitle.Trim().Replace(" ", "-");

            await articleRepository.UpdateAsync(article);

            ArticleDto articleDto = mapper.Map<ArticleDto>(article);

            return articleDto;
        }

        public async Task DeleteArticle(string slug, string CurrentUserName)
        {
            withRelatedData = false;

            Article articleToDelete = await GetArticle(slug, withRelatedData);

            checkUserPermission(CurrentUserName, articleToDelete.User.Username);

            await articleRepository.DeleteAsync(articleToDelete);
        }

        public async Task<CommentDto> AddCommentAsync(CommentCreationDto comment, string slug, string CurrentUserName)
        {
            withRelatedData = false;

            Article article = await GetArticle(slug, withRelatedData);
            User commentOwner = await userRepository.GetUserByUsername(CurrentUserName);

            Comment addedComment = MapToComment(comment, article, commentOwner);

            await commentRepository.CreateAsync(addedComment);

            CommentDto commentDto = mapper.Map<CommentDto>(addedComment);

            return commentDto;
        }

        public async Task DeleteComment(int commentId, string slug, string CurrentUserName)
        {
            Comment comment = await GetComment(commentId);

            checkUserPermission(CurrentUserName, comment.User.Username);

            await commentRepository.DeleteAsync(comment);
        }

        public async Task<ArticleDto> AddToFavorites(string slug, string currentUserName)
        {
            User currentUser = await userRepository.GetUserWithFollowings(currentUserName);

            Article favoritedArticle = await GetArticle(slug, true);

            bool isFavorited = CheckArticleIfFavorited(favoritedArticle, currentUser);

            if (isFavorited)
            {
                throw new ConflictException("You already favorite this article");
            }

            await articleRepository.FavoriteArticle(favoritedArticle, currentUser);

            var articleDto = mapper.Map<ArticleDto>(favoritedArticle);

            articleDto.Favorited = true;

            MapFollowAndFavoriteStatus(favoritedArticle, currentUser, articleDto);

            return articleDto;
        }

        public async Task<ArticleDto> RemoveFromFavorites(string slug, string currentUserName)
        {
            User currentUser = await userRepository.GetUserWithFollowings(currentUserName);

            Article favoritedArticle = await GetArticle(slug, true);

            bool isFavorited = CheckArticleIfFavorited(favoritedArticle, currentUser);

            if (!isFavorited)
            {
                throw new ConflictException("You never favorite this article");
            }

            await articleRepository.UnFavoriteArticle(favoritedArticle, currentUser);

            var articleDto = mapper.Map<ArticleDto>(favoritedArticle);

            MapFollowAndFavoriteStatus(favoritedArticle, currentUser, articleDto);

            return articleDto;
        }

        public async Task<List<ArticleDto>> GetGlobalFeed
            (int limit, int offset, string tag, string author)
        {
            List<Article> articles;

            GetFilter(tag, author, out Expression<Func<Article, bool>>? filterExpression);

            articles = await articleRepository.GetFeed(filterExpression, limit, offset);

            return mapper.Map<List<ArticleDto>>(articles);
        }

        public async Task<List<ArticleDto>> GetUserFeed
            (int limit, int offset, string tag, string author, string currentUserName)
        {
            List<Article> articles;

            GetFilter(tag, author, out Expression<Func<Article, bool>>? func);

            User currentUser = await userRepository.GetUserWithFollowings(currentUserName);

            articles = await articleRepository.GetFeed(func, limit, offset, currentUser);

            return MapToArticleDtoList(articles, currentUser);
        }

        public async Task<List<ArticleDto>> GetFavorites(string currentUserName, int limit, int offset)
        {
            User currentUser = await userRepository.GetUserWithFollowings(currentUserName);

            List<Article> articles = await articleRepository.GetFavoriteArticles(currentUser, limit, offset);

            return MapToArticleDtoList(articles, currentUser);
        }

        public async Task<List<CommentDto>> GetComments(string slug)
        {
            var article = await GetArticle(slug, false);

            List<Comment> comments = await commentRepository.GetComments(article);

            return mapper.Map<List<CommentDto>>(comments);
        }

        private void MapFollowAndFavoriteStatus(Article article, User currentUser, ArticleDto articleDto)
        {
            articleDto.UserProfile.Following = CheckFollowStatusWithPublisher(currentUser, article.UserId, articleDto);
            articleDto.Favorited = CheckArticleIfFavorited(article, currentUser);
        }

        private static void GetFilter(string tag, string author, out Expression<Func<Article, bool>>? filterExpression)
        {
            filterExpression = null;

            if (tag != null)
            {
                filterExpression = article => article.Tags.Any(t => t.TagName == tag);
            }
            else if (author != null)
            {
                filterExpression = article => article.User.Username == author;
            }
        }

        private bool CheckArticleIfFavorited(Article article, User user)
        {
            return article.Favorites.Any(f => f.UserId == user.UserId);
        }

        private async Task<Comment> GetComment(int commentId)
        {
            try
            {
                return await commentRepository.GetCommentById(commentId);
            }
            catch (Exception)
            {
                throw new NotFoundException("The comment you're trying to delete doesn't exist");
            }
        }

        private Comment MapToComment(CommentCreationDto comment, Article article, User commentOwner)
        {
            Comment addedComment = mapper.Map<Comment>(comment);
            addedComment.Article = article;
            addedComment.User = commentOwner;
            return addedComment;
        }

        private bool CheckFollowStatusWithPublisher(User currentUser, int publisherId, ArticleDto articleDto)
        {
            return profileService.CheckFollowStatus(publisherId, currentUser);
        }

        private async Task<Article> GetArticle(string slug, bool withRelatedData)
        {
            try
            {
                if (withRelatedData)
                {
                    return await articleRepository.GetArticleWithRelatedDataAsync(slug);
                }
                else
                {
                    return await articleRepository.GetArticle(slug);
                }
            }
            catch (Exception)
            {
                throw new NotFoundException("The article you requested doesn't exist");
            }
        }


        private async Task AddTags(List<string> tags, Article article)
        {
            var ArticleTags = new List<Tag>();
            
            foreach (string tag in tags)
            {
                Tag Tag;
                try
                {
                    Tag = await tagRepository.GetTag(tag);
                }
                catch (Exception)
                {
                    Tag = await tagRepository.CreateTag(new Tag { TagName = tag });
                }
                ArticleTags.Add(Tag);
            }
            await articleRepository.AddTagsToArticle(ArticleTags, article);
        }

        private static void checkUserPermission(string currentUserName, string publisherName)
        {
            if (currentUserName != publisherName)
            {
                throw new ForbiddenOperationException();
            }
        }

        private List<ArticleDto> MapToArticleDtoList(List<Article> articles, User currentUser)
        {
            var articlesDtos = new List<ArticleDto>();
            articles.ForEach(a =>
            {
                var dto = mapper.Map<ArticleDto>(a);
                MapFollowAndFavoriteStatus(a, currentUser, dto);
                articlesDtos.Add(dto);
            });

            return articlesDtos;
        }
    }
}
