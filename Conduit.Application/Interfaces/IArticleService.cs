﻿using Conduit.Domain.DTOs;
using System.Runtime.InteropServices;

namespace Conduit.Application.Interfaces
{
    public interface IArticleService
    {
        Task<ArticleDto> AddArticle(ArticleCreationDto articleDetails, string Username);
        Task<CommentDto> AddCommentAsync(CommentCreationDto comment, string slug, string CurrentUserName);
        Task DeleteArticle(string slug, string CurrentUserName);
        Task DeleteComment(int commentId, string slug, string CurrentUserName);
        Task<ArticleDto> GetArticle(string slug, [Optional] string CurrentUserName);
        Task<ArticleDto> UpdateArticle(string slug, ArticleUpdateDto articleUpdates, string CurrentUserName);
    }
}
