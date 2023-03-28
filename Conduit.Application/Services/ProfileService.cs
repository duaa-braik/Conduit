using AutoMapper;
using Conduit.Application.Interfaces;
using Conduit.Domain.DTOs;
using Conduit.Domain.Entities;
using Conduit.Domain.Interfaces;
using Conduit.Domain.Profiles;
using System.Runtime.InteropServices;

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

        public async Task<UserProfileDto> GetUserProfile(string Username)
        {
            var User = await userRepository.GetUserByUsername(Username);

            return mapper.Map<UserProfileDto>(User);
        }

        public async Task<UserProfileDto> GetUserProfile(string Username, string CurrentUserName)
        {
            var User = await userRepository.GetUserByUsername(Username);

            var CurrentUser = await userRepository.GetUserWithFollowings(CurrentUserName);

            bool isFollowing = CheckFollowStatus(User.UserId, CurrentUser);

            var UserProfile = mapper.Map<UserProfileDto>(User);

            UserProfile.Following = isFollowing;

            return UserProfile;
        }

        private bool CheckFollowStatus(int userId, User currentUser)
        {
            return currentUser.Followings.Any(user => user.FolloweeId == userId);
        }
    }
}
