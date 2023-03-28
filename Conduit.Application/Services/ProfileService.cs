using AutoMapper;
using Conduit.Application.Interfaces;
using Conduit.Domain.DTOs;
using Conduit.Domain.Entities;
using Conduit.Domain.Exceptions;
using Conduit.Domain.Interfaces;

namespace Conduit.Application.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        private class UsersRelationship
        {
            public User Follower { get; set; }
            public User Followee { get; set; }
            public bool IsFollowing { get; set; }
        }

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
            var Users = await GetUsers(Username, CurrentUserName);

            var User = Users.Followee;

            bool isFollowing = Users.IsFollowing;

            var UserProfile = mapper.Map<UserProfileDto>(User);

            UserProfile.Following = isFollowing;

            return UserProfile;
        }

        public async Task<UserProfileDto> FollowUser(string Username, string CurrentUserName)
        {
            var Users = await GetUsers(Username, CurrentUserName);

            if (Users.IsFollowing)
            {
                throw new FollowStatusMatchException("You already follow this user");
            }

            var FollowedUser = await userRepository.FollowUser(Users.Followee, Users.Follower);

            var FollowedUserProfile = mapper.Map<UserProfileDto>(FollowedUser);
            FollowedUserProfile.Following = true;

            return FollowedUserProfile;

        }

        private async Task<UsersRelationship> GetUsers(string FolloweeName, string FollowerName)
        {
            var User = await userRepository.GetUserByUsername(FolloweeName);

            var CurrentUser = await userRepository.GetUserWithFollowings(FollowerName);

            bool isFollowing = CheckFollowStatus(User.UserId, CurrentUser);

            return new UsersRelationship
            {
                Followee = User,
                Follower = CurrentUser,
                IsFollowing = isFollowing
            };
        }

        private bool CheckFollowStatus(int userId, User currentUser)
        {
            return currentUser.Followings.Any(user => user.FolloweeId == userId);
        }
    }
}
