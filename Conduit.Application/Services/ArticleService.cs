using Conduit.Application.Interfaces;
using Conduit.Domain.Interfaces;

namespace Conduit.Application.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository articleRepository;

        public ArticleService(IArticleRepository articleRepository)
        {
            this.articleRepository = articleRepository;
        }
    }
}
