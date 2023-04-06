using Conduit.Application.Services;
using Conduit.Domain.DTOs;
using Conduit.Domain.Exceptions;
using Conduit.Tests.Mocks;

namespace Conduit.Tests.Tests
{
    public class ProfileServiceTests
    {
        public MapperMocks mapperMocks { get; set; }
        public UserRepositoryMock repositoryMocks { get; set; }

        private readonly ProfileService profileService;

        public ProfileServiceTests()
        {
            mapperMocks = new MapperMocks();
            repositoryMocks = new UserRepositoryMock();

            profileService = new ProfileService(repositoryMocks.userRepositoryMock.Object, mapperMocks.MapperMock.Object);
        }

        [Theory]
        [InlineData("braik", "duaa", true)]
        [InlineData("duaa", "braik", false)]
        [InlineData("duaa_braik", "duaa", false)]
        public async void GetUserProfileTest_MustReturnUserProfileWithFollowStatus
            (string username, string currentUsername, bool expectedFollowStatus)
        {
            UserProfileDto userProfile = await profileService.GetUserProfile(username, currentUsername);

            Assert.NotNull(userProfile);
            Assert.Equal(username, userProfile.UserName);
            Assert.Equal(expectedFollowStatus, userProfile.Following);
        }

        [Fact]
        public async void FollowUserTest_MustReturnTheFollowedUser()
        {
            var currentUsername = "duaa";
            var userToFollow = "duaa_braik";

            var userProfile = await profileService.FollowUser(userToFollow, currentUsername);

            Assert.NotNull(userProfile);
            Assert.True(userProfile.Following);
        }

        [Fact]
        public async void FollowUserTest_MustThrowAnExceptionIfUserIsAlreadyFollowed()
        {
            var currentUsername = "duaa";
            var userToFollow = "braik";

            await Assert.ThrowsAsync<ConflictException>(() => profileService.FollowUser(userToFollow, currentUsername));
        }

        [Fact]
        public async void UnFollowUserTest_MustReturnTheUnfollowedUser()
        {
            var currentUsername = "duaa";
            var userToFollow = "braik";

            var userProfile = await profileService.UnFollowUser(userToFollow, currentUsername);

            Assert.NotNull(userProfile);
            Assert.False(userProfile.Following);
        }

        [Fact]
        public async void UnFollowUserTest_MustThrowAnExceptionIfUserIsAlreadyUnFollowed()
        {
            var currentUsername = "duaa";
            var userToFollow = "duaa_braik";

            await Assert.ThrowsAsync<ConflictException>(() => profileService.UnFollowUser(userToFollow, currentUsername));
        }
    }
}
