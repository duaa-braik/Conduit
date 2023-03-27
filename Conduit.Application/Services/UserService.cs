using AutoMapper;
using Conduit.Application.Interfaces;
using Conduit.Domain.DTOs;
using Conduit.Domain.Entities;
using Conduit.Domain.Interfaces;

namespace Conduit.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<UserAuthenticationDto> Register(UserDto userRegistrationInfo)
        {
            User User = mapper.Map<User>(userRegistrationInfo);

            User = await userRepository.CreateAsync(User);

            return mapper.Map<UserAuthenticationDto>(User);
        }
    }
}
