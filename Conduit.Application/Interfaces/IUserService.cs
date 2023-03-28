using Conduit.Domain.DTOs;

namespace Conduit.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserAuthenticationDto> GetCurrentUser(string Email);
        Task<UserDto> Login(UserLoginDto userLoginInfo);
        UserAuthenticationDto MapToUserAuthenticationDto(UserDto userDto);
        Task<UserAuthenticationDto> Register(UserDto userRegistrationInfo);
    }
}
