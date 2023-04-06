using Conduit.Application.Services;
using Conduit.Domain.DTOs;
using Conduit.Domain.Exceptions;
using Conduit.Tests.Mocks;

namespace Conduit.Tests.Tests
{
    public class ArticleServiceTests
    {
        public MapperMocks mapperMocks { get; set; }
        public UserRepositoryMock userRepository { get; set; }
        public ArticleRepositoryMocks ArticleRepository { get; set; }
        public TagRepositoryMocks TagRepository { get; set; }
        public CommentRepositoryMocks CommentRepository { get; set; }
        public ProfileServiceMocks profileService { get; set; }

        private ArticleService articleService { get; set; }

        public ArticleServiceTests()
        {
            mapperMocks = new MapperMocks();
            userRepository = new UserRepositoryMock();
            ArticleRepository = new ArticleRepositoryMocks();
            TagRepository = new TagRepositoryMocks();
            CommentRepository = new CommentRepositoryMocks();
            profileService = new ProfileServiceMocks();

            articleService = new ArticleService
                (ArticleRepository.articleRepositoryMock.Object, 
                userRepository.userRepositoryMock.Object,
                mapperMocks.MapperMock.Object, 
                TagRepository.TagRepositoryMock.Object,
                profileService.profileServiceMock.Object, 
                CommentRepository.CommentRepositoryMock.Object);
        }

        [Fact]
        public async void AddArticleTest_MustReturnTheAddedArticle()
        {
            ArticleCreationDto article = new() { Title = "Test", Description = "test", Text = "test", TagList =  new List<string> { "lorem", "test" } };

            ArticleDto articleDto = await articleService.AddArticle(article, "duaa");

            Assert.NotNull(articleDto);
            Assert.True(articleDto.Title.Equals("Test"));
            Assert.NotEmpty(article.TagList);
            Assert.Contains("lorem", articleDto.TagList);
            Assert.Contains("test", articleDto.TagList);
            Assert.Equal("duaa", articleDto.UserProfile.UserName);
        }

        [Fact]
        public async void GetArticleTest_MustReturnTheFoundArticle()
        {
            string slug = "Intro-to-computer-science";

            ArticleDto articleDto = await articleService.GetArticle(slug);

            Assert.NotNull(articleDto);
            Assert.Equal(slug, articleDto.Slug);
        }

        [Fact]
        public async void GetArticleTest_MustThrowNotFoundExceptionIfNotFound()
        {
            string slug = "Intro-to-computer-sciencee";

            await Assert.ThrowsAsync<NotFoundException>(() => articleService.GetArticle(slug));
        }

        [Fact]
        public async void UpdateArticleTest_MustReturnTheUpdatedArticle_IfUpdatedByTheOwner()
        {
            ArticleUpdateDto articleUpdates = new() { Title = "Functional Programming" };

            string username = "duaa";
            string slug = "Intro-to-computer-science";
            string updatedSlug = "Functional-Programming";

            ArticleDto articleDto = await articleService.UpdateArticle(slug, articleUpdates, username);

            Assert.Equal(articleUpdates.Title, articleDto.Title);
            Assert.Equal(updatedSlug, articleDto.Slug);
        }

        [Fact]
        public async void UpdateArticleTest_MustThrowForbiddenOperationException_IfUpdatedByAnotherUser()
        {
            ArticleUpdateDto articleUpdates = new() { Title = "Functional Programming" };

            string username = "braik";
            string slug = "Intro-to-computer-science";

            await Assert.ThrowsAsync<ForbiddenOperationException>(
                () => articleService.UpdateArticle(slug, articleUpdates, username));
        }

        [Fact]
        public async void AddToFavoritesTest_MustMarkTheArticleAsFavorited()
        {
            string slug = "Object-Oriented-Programming";
            string username = "duaa";

            ArticleDto articleDto = await articleService.AddToFavorites(slug, username);

            Assert.True(articleDto.Favorited);
        }

        [Fact]
        public async void AddToFavoritesTest_MudtThrowConflictException()
        {
            string slug = "Intro-to-computer-science";
            string username = "duaa";

            await Assert.ThrowsAsync<ConflictException>(() => articleService.AddToFavorites(slug, username)); 
        }
    }
}
