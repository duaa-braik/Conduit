using AutoMapper;
using Conduit.Domain.DTOs;
using Conduit.Domain.Entities;

namespace Conduit.Domain.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<User, UserAuthenticationDto>();
        }
    }
}
