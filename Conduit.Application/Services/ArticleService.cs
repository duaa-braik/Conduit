using Conduit.Application.Interfaces;
using Conduit.Domain.DTOs;
using Conduit.Domain.Interfaces;

namespace Conduit.Application.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository articleRepository;
        private readonly IUserRepository userRepository;

        public ArticleService(IArticleRepository articleRepository, IUserRepository userRepository)
        {
            this.articleRepository = articleRepository;
            this.userRepository = userRepository;
        }

        public async Task<ArticleDto> AddArticle(ArticleCreationDto articleDetails, string Username)
        {

        }
    }
}
