using AutoMapper;
using Conduit.Application.Interfaces;
using Conduit.Domain.DTOs;
using Conduit.Domain.Interfaces;

namespace Conduit.Application.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public ProfileService(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<UserProfileDto> GetUserProfile(string Username, string CurrentUserName)
        {
            var User = await userRepository.GetUserByUsername(Username);

            var CurrentUser = await userRepository.GetUserWithFollowings(CurrentUserName);

            var UserProfile = mapper.Map<UserProfileDto>(User);

            return mapper.Map<UserProfileDto>(User);
        }
    }
}
