using Conduit.Domain.DTOs;
using Conduit.Domain.Entities;

namespace Conduit.Application.Interfaces
{
    public interface IProfileService
    {
        bool CheckFollowStatus(int userId, User currentUser);
        Task<UserProfileDto> FollowUser(string Username, string CurrentUserName);
        Task<UserProfileDto> GetUserProfile(string Username);
        Task<UserProfileDto> GetUserProfile(string Username, string CurrentUserName);
        Task<UserProfileDto> UnFollowUser(string Username, string CurrentUserName);
    }
}
