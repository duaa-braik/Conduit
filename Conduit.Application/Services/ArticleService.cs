using AutoMapper;
using Conduit.Application.Interfaces;
using Conduit.Domain.DTOs;
using Conduit.Domain.Entities;
using Conduit.Domain.Interfaces;

namespace Conduit.Application.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository articleRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly ITagRepository tagRepository;

        public ArticleService
            (IArticleRepository articleRepository, IUserRepository userRepository, IMapper mapper, ITagRepository tagRepository)
        {
            this.articleRepository = articleRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.tagRepository = tagRepository;
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
    }
}
