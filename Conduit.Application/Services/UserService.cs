using Conduit.Domain.Interfaces;

namespace Conduit.Application.Services
{
    public class UserService
    {
        private readonly IUserRpository userRepository;

        public UserService(IUserRpository userRepository)
        {
            this.userRepository = userRepository;
        }
    }
}
