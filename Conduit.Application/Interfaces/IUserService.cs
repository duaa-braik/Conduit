using Conduit.Domain.DTOs;

namespace Conduit.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> Login(UserLoginDto userLoginInfo);
        Task<UserAuthenticationDto> Register(UserDto userRegistrationInfo);
    }
}
