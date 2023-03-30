using AutoMapper;
using Conduit.Application.Interfaces;
using Conduit.Domain.DTOs;
using Conduit.Domain.Entities;
using Conduit.Domain.Exceptions;
using Conduit.Domain.Interfaces;
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
        private bool withRelatedData;

        public ArticleService
            (IArticleRepository articleRepository, IUserRepository userRepository, 
            IMapper mapper, ITagRepository tagRepository, IProfileService profileService)
        {
            this.articleRepository = articleRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.tagRepository = tagRepository;
            this.profileService = profileService;
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

        public async Task<ArticleDto> GetArticle(string slug, [Optional] string CurrentUserName)
        {
            withRelatedData = true;
            Article article = await GetArticle(slug, withRelatedData);

            var tags = article.Tags.Select(t => t.TagName).ToList();

            ArticleDto articleDto = mapper.Map<ArticleDto>(article);

            if (CurrentUserName != null)
            {
                User currentUser = await userRepository.GetUserWithFollowings(CurrentUserName);
                CheckFollowStatusWithPublisher(currentUser, article.UserId, articleDto);
            }

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

        private void CheckFollowStatusWithPublisher(User currentUser, int publisherId, ArticleDto articleDto)
        {
            bool isFollowing = profileService.CheckFollowStatus(publisherId, currentUser);
            articleDto.UserProfile.Following = isFollowing;
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
    }
}
