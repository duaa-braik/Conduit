using Conduit.Application.Interfaces;
using Conduit.Domain.Entities;
using Moq;

namespace Conduit.Tests.Mocks
{
    public class ProfileServiceMocks
    {
        public Mock<IProfileService> profileServiceMock { get; set; }

        public ProfileServiceMocks()
        {
            profileServiceMock = new Mock<IProfileService>();

            profileServiceMock.Setup(x => x.CheckFollowStatus(It.IsAny<int>(), It.IsAny<User>()))
                .Returns((int userId, User user) => user.Followings.Any(f => f.FolloweeId == userId));
        }
    }
}
