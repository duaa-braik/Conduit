using Conduit.Application.Interfaces;
using Conduit.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ArticleDto>> AddArticle(ArticleCreationDto article)
        {
            var userName = User.Claims.FirstOrDefault(claim => claim.Type == "UserName")!.Value;

            var publishedArticle = await articleService.AddArticle(article, userName);

            return Created($"/api/articles/{publishedArticle.Slug}", publishedArticle);
        }

        [HttpGet]
        [Route("{slug}")]
        public async Task<ActionResult<ArticleDto>> GetArticle(string slug)
        {
            var UserNameClaim = User.Claims.FirstOrDefault(claim => claim.Type == "UserName");
            ArticleDto article;
            if(UserNameClaim == null)
            {
                article = await articleService.GetArticle(slug);
            }
            else
            {
                article = await articleService.GetArticle(slug, UserNameClaim.Value);
            }
            return Ok(article);
        }
    }
}
