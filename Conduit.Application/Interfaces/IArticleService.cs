using Conduit.Domain.DTOs;

namespace Conduit.Application.Interfaces
{
    public interface IArticleService
    {
        Task<ArticleDto> AddArticle(ArticleCreationDto articleDetails, string Username);
    }
}
