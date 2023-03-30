using Conduit.Domain.DTOs;
using System.Runtime.InteropServices;

namespace Conduit.Application.Interfaces
{
    public interface IArticleService
    {
        Task<ArticleDto> AddArticle(ArticleCreationDto articleDetails, string Username);
        Task<ArticleDto> GetArticle(string slug, [Optional] string CurrentUserName);
    }
}
