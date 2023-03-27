using Conduit.Domain.DTOs;

namespace Conduit.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserAuthenticationDto> Register(UserDto userRegistrationInfo);
    }
}
