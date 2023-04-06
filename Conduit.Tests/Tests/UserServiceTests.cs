using Conduit.Application.Services;
using Conduit.Domain.DTOs;
using Conduit.Domain.Exceptions;
using Conduit.Tests.Mocks;
using EntityFramework.Exceptions.Common;

namespace Conduit.Tests.Tests
{
    public class UserServiceTests
    {
        public MapperMocks mapperMocks { get; set; }
        public UserRepositoryMock userRepositoryMocks { get; set; }

        private readonly UserService userService;

        public UserServiceTests()
        {
            mapperMocks = new MapperMocks();
            userRepositoryMocks = new UserRepositoryMock();
            userService = new UserService(userRepositoryMocks.userRepositoryMock.Object, mapperMocks.MapperMock.Object);
        }

        [Theory]
        [InlineData("duaaB@gmail.com", "hi")]
        [InlineData("duaa@gmail.com", "hi this is duaa")]
        [InlineData("duaaB@gmail.com", "hi this is duaa")]
        public async void UpdateUserTest_MustReturnUpdatedUser(string email, string bio)
        {
            UserUpdateDto userUpdateDto = new() { Email = email, Bio = bio };
            string UserEmail = "duaa@gmail.com";

            var userAuthenticationDto = await userService.UpdateUser(userUpdateDto, UserEmail);

            Assert.Equal(email, userAuthenticationDto.Email);
            Assert.Equal(bio, userAuthenticationDto.Bio);
        }

        [Theory]
        [InlineData("duaa66@gmail.com", "", "This account doesn't exist")]
        [InlineData("duaa@gmail.com", "1234", "Wrong password")]
        public async void UserLoginTest_MustThrowExceptionOnLoginError(string email, string password, string errorMessage)
        {
            UserLoginDto userCredentials = new() { Email = email, Password = password};

            var Exception = await Assert.ThrowsAsync<LoginFailureException>(() => userService.Login(userCredentials));

            Assert.Equal(errorMessage, Exception.Message);
        }

        [Fact]
        public async void UserRegisterationTest()
        {
            UserDto userInfo = new() { Email = "duaa12@gmail.com", UserName = "ss", Password = "ss" };

            UserAuthenticationDto userDto = await userService.Register(userInfo);

            Assert.Equal(userDto.Email, userInfo.Email);
        }

        [Theory]
        [InlineData("duaa12@gmail.com", "duaa")]
        [InlineData("duaa@gmail.com", "duaa__")]
        [InlineData("duaa@gmail.com", "duaa")]
        public async void UserRegisterationTest_ThrowsExceptionOnDuplicateEmailOrUsername(string email, string username)
        {
            UserDto userInfo = new() { Email = email, UserName = username, Password = "ss" };

            await Assert.ThrowsAsync<UniqueConstraintException>(() => userService.Register(userInfo));
        }
    }
}
