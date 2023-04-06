using Conduit.Domain.Entities;
using Conduit.Domain.Exceptions;
using Conduit.Domain.Interfaces;
using EntityFramework.Exceptions.Common;
using Moq;
using System.Data;

namespace Conduit.Tests.Mocks
{
    public class UserRepositoryMock : FakeDatabase
    {
        public Mock<IUserRepository> userRepositoryMock { get; set; }

        public UserRepositoryMock()
        {
            userRepositoryMock = new Mock<IUserRepository>();

            userRepositoryMock.Setup(x => x.CreateAsync
            (It.Is<User>(user => 
            !users.Select(u => u.Email).Contains(user.Email) && !users.Select(u => u.Email).Contains(user.Username))))
                .ReturnsAsync((User user) => user);

            userRepositoryMock.Setup(x => x.CreateAsync
            (It.Is<User>(user => users.Select(u => u.Email).Contains(user.Email) || users.Select(u => u.Username).Contains(user.Username))))
                .ThrowsAsync(new UniqueConstraintException());

            userRepositoryMock.Setup(x => x.GetUserByEmail(It.IsAny<string>()))
                .ReturnsAsync((string email) => users.First(u => u.Email == email));

            userRepositoryMock.Setup(x => x.GetUserByUsername(It.IsAny<string>()))
                .ReturnsAsync((string username) => users.First(u => u.Username == username));

            userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync((User updatedUser) => updatedUser);

            userRepositoryMock.Setup(x => x.GetUserWithFollowings(It.IsAny<string>()))
                .ReturnsAsync((string username) => users.First(u => u.Username == username));

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
