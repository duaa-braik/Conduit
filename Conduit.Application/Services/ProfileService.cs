using Conduit.Application.Interfaces;
using Conduit.Domain.Interfaces;

namespace Conduit.Application.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserRepository userRepository;

        public ProfileService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
    }
}
