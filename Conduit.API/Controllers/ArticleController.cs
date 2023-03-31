using Conduit.Application.Interfaces;
using Conduit.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            string userName = GetUserNameClaim().Value;

            var publishedArticle = await articleService.AddArticle(article, userName);

            return Created($"/api/articles/{publishedArticle.Slug}", publishedArticle);
        }

        [HttpGet]
        [Route("{slug}")]
        public async Task<ActionResult<ArticleDto>> GetArticle(string slug)
        {
            var UserNameClaim = GetUserNameClaim();
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

        [HttpPut]
        [Authorize]
        [Route("{slug}")]
        public async Task<ActionResult<ArticleDto>> Update(string slug, ArticleUpdateDto updates)
        {
            string userName = GetUserNameClaim().Value;
            var a = await articleService.UpdateArticle(slug, updates, userName);
            return Ok(a);
        }

        [HttpDelete]
        [Authorize]
        [Route("{slug}")]
        public async Task<ActionResult> DeleteArticle(string slug)
        {
            string userName = GetUserNameClaim().Value;
            await articleService.DeleteArticle(slug, userName);
            return NoContent();
        }

        [HttpPost]
        [Authorize]
        [Route("{slug}/comments", Name = "AddComment")]
        public async Task<ActionResult> AddComment(string slug, CommentCreationDto comment)
        {
            string userName = GetUserNameClaim().Value;
            var addedComment = await articleService.AddCommentAsync(comment, slug, userName);
            return Created("AddComment", addedComment);
        }

        [HttpDelete]
        [Authorize]
        [Route("{slug}/comments/{Id}", Name = "DeleteComment")]
        public async Task<ActionResult> DeleteComment(string slug, int Id)
        {
            string userName = GetUserNameClaim().Value;
            await articleService.DeleteComment(Id, slug, userName);
            return NoContent();
        }

        [HttpPost]
        [Authorize]
        [Route("{slug}/favorites")]
        public async Task<ActionResult> Favorite(string slug)
        {
            string userName = GetUserNameClaim().Value;
            var article = await articleService.AddToFavorites(slug, userName);
            return Ok(article);
        }

        private Claim GetUserNameClaim()
        {
            return User.Claims.FirstOrDefault(claim => claim.Type == "UserName");
        }
    }
}
