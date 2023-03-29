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

        public ArticleService
            (IArticleRepository articleRepository, IUserRepository userRepository, IMapper mapper)
        {
            this.articleRepository = articleRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<ArticleDto> AddArticle(ArticleCreationDto articleDetails, string Username)
        {
            User Publisher = await userRepository.GetUserByUsername(Username);

            Article article = mapper.Map<Article>(articleDetails);

            article.User = Publisher;

            await articleRepository.CreateAsync(article);

            ArticleDto articleDto = mapper.Map<ArticleDto>(article);

            return articleDto;
        }
    }
}
