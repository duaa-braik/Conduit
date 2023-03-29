using Conduit.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API.Controllers
{
    [ApiController]
    [Route("/api/articles")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService articleService;

        public ArticleController(IArticleService articleService)
        {
            this.articleService = articleService;
        }
    }
}
