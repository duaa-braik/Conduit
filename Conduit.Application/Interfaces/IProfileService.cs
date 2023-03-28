using Conduit.Domain.DTOs;
using System.Runtime.InteropServices;

namespace Conduit.Application.Interfaces
{
    public interface IProfileService
    {
        Task<UserProfileDto> GetUserProfile(string Username);
        Task<UserProfileDto> GetUserProfile(string Username, string CurrentUserName);
    }
}
