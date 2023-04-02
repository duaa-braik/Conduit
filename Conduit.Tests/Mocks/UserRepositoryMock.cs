using Conduit.Domain.Entities;
using Conduit.Domain.Exceptions;
using Conduit.Domain.Interfaces;
using EntityFramework.Exceptions.Common;
using Moq;
using System.Data;

namespace Conduit.Tests.Mocks
{
    public class UserRepositoryMock
    {
        public Mock<IUserRepository> userRepositoryMock { get; set; }

        List<User> users;

        public UserRepositoryMock()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            users = new List<User>()
            {
                new User { UserId = 1, Username = "duaa", Email = "duaa@gmail.com", Bio = "hii", Password = "123"},
                new User { UserId = 2, Username = "braik", Email = "duaa1@gmail.com", Bio = "hii", Password = "123"},
                new User { UserId = 3, Username = "duaa_braik", Email = "duaa2@gmail.com", Bio = "hii", Password = "123"}
            };

            users[0].Followings.Add(new Follow { FolloweeId = 2, FollowerId = 1});

            userRepositoryMock.Setup(x => x.CreateAsync(It.Is<User>(user => !users.Select(u => u.Email).Contains(user.Email))))
                .ReturnsAsync((User user) => user);

            userRepositoryMock.Setup(x => x.CreateAsync
            (It.Is<User>(user => users.Select(u => u.Email).Contains(user.Email) || users.Select(u => u.Username).Contains(user.Username))))
                .ThrowsAsync(new UniqueConstraintException());

            userRepositoryMock.Setup(x => x.GetUserByEmail(It.IsIn(users.Select(u => u.Email))))
                .ReturnsAsync((string email) => users.First(u => u.Email == email));

            userRepositoryMock.Setup(x => x.GetUserByEmail(It.IsNotIn(users.Select(u => u.Email))))
                .ThrowsAsync(new NotFoundException());

            userRepositoryMock.Setup(x => x.GetUserByUsername(It.IsIn(users.Select(u => u.Username))))
                .ReturnsAsync((string username) => users.First(u => u.Username == username));

            userRepositoryMock.Setup(x => x.GetUserByUsername(It.IsNotIn(users.Select(u => u.Username))))
                .ThrowsAsync(new NotFoundException());

            userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync((User updatedUser) => updatedUser);

            userRepositoryMock.Setup(x => x.GetUserWithFollowings(It.IsIn(users.Select(u => u.Username))))
                .ReturnsAsync((string username) => users.First(u => u.Username == username));

            userRepositoryMock.Setup(x => x.GetUserWithFollowings(It.IsNotIn(users.Select(u => u.Username))))
                .ThrowsAsync(new NotFoundException());

            userRepositoryMock.Setup(x => x.FollowUser(It.IsAny<User>(), It.IsAny<User>()))
                .ReturnsAsync((User user, User userToFollow) =>
                {
                    user.Followings.Add(new Follow { FolloweeId = userToFollow.UserId, FollowerId = user.UserId });
                    return userToFollow;
                });

            userRepositoryMock.Setup(x => x.UnFollowUser(It.IsAny<User>(), It.IsAny<User>()))
                .ReturnsAsync((User user, User userToFollow) =>
                {
                    var follow = users
                        .FirstOrDefault(u => u.Username == user.Username)!
                        .Followings.FirstOrDefault(f => f.FolloweeId == userToFollow.UserId);

                    user.Followings.Remove(follow);

                    return userToFollow;
                });
        }
    }
}
