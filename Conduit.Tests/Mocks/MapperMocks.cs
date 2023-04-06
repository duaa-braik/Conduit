using AutoMapper;
using Conduit.Domain.DTOs;
using Conduit.Domain.Entities;
using Moq;

namespace Conduit.Tests.Mocks
{
    public class MapperMocks
    {
        public Mock<IMapper> MapperMock { get; set; }
        public MapperMocks()
        {
            MapperMock = new Mock<IMapper>();

            MapperMock.Setup(x => x.Map<User>(It.IsAny<UserDto>()))
                .Returns((UserDto userDto) =>
                new User { Username = userDto.UserName, Email = userDto.Email, Password = userDto.Password });

            MapperMock.Setup(x => x.Map <UserAuthenticationDto>(It.IsAny<User>()))
                .Returns((User user) =>
                new UserAuthenticationDto { Bio = user.Bio, Email = user.Email, UserName = user.Username, Token = "Any token" });

            MapperMock.Setup(x => x.Map<UserDto>(It.IsAny<User>()))
                .Returns((User user) => new UserDto { UserName = user.Username, Email = user.Email, Password = user.Password });

            MapperMock.Setup(x => x.Map<UserProfileDto>(It.IsAny<User>()))
                .Returns((User user) => new UserProfileDto { UserName = user.Username, Bio = user.Bio });

            MapperMock.Setup(x => x.Map(It.IsAny<UserUpdateDto>(), It.IsAny<User>()))
                .Returns((UserUpdateDto userUpdates, User user) =>
                {
                    user.Email = userUpdates.Email;
                    user.Bio = userUpdates.Bio;
                    return user;
                });

            MapperMock.Setup(x => x.Map<Article>(It.IsAny<ArticleCreationDto>()))
                .Returns((ArticleCreationDto article)
                => new Article
                {
                    Title = article.Title,
                    Description = article.Description,
                    Text = article.Text,
                    DatePublished = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow,
                    Slug = article.Title.Trim().Replace(" ", "-")
                });

            MapperMock.Setup(x => x.Map<ArticleDto>(It.IsAny<Article>()))
                .Returns((Article article) => new ArticleDto
                {
                    Title = article.Title,
                    Description = article.Description,
                    Text = article.Text,
                    DatePublished = article.DatePublished,
                    LastModified = article.LastModified,
                    FavoritesCount = article.Favorites.Count,
                    Slug = article.Slug,
                    TagList = article.Tags.Select(t => t.TagName).ToList(),
                    UserProfile = new UserProfileDto { Bio = article.User.Bio, UserName = article.User.Username }
                });

            MapperMock.Setup(x => x.Map<Comment>(It.IsAny<CommentCreationDto>()))
                .Returns((CommentCreationDto comment) => new Comment { Body = comment.body, CreatedAt = DateTime.UtcNow });

            MapperMock.Setup(x => x.Map<CommentDto>(It.IsAny<Comment>()))
                .Returns((Comment comment)
                => new CommentDto
                {
                    Body = comment.Body,
                    CommentId = comment.CommentId,
                    CreatedAt = comment.CreatedAt,
                    UserProfile = new UserProfileDto { Bio = comment.User.Bio, UserName = comment.User.Username }
                });

        }
    }
}
