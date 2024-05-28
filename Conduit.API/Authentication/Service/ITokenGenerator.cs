using Conduit.Domain.DTOs;

namespace Conduit.API.Authentication.Service
{
    public interface ITokenGenerator
    {
        public string Generate(UserDto userModel, JwtSettings jwtSettings);
    }
}
