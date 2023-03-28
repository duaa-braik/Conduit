using Conduit.Domain.DTOs;

namespace Conduit.Application.Interfaces
{
    public interface IProfileService
    {
        Task<UserProfileDto> GetUserProfile(string Username, string CurrentUserName);
    }
}
