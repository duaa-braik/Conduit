using AutoMapper;
using Conduit.Application.Interfaces;
using Conduit.Domain.DTOs;
using Conduit.Domain.Entities;
using Conduit.Domain.Exceptions;
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

        public async Task<UserDto> Login(UserLoginDto userLoginInfo)
        {
            string Email = userLoginInfo.Email;
            string Password = userLoginInfo.Password;

            User UserInDatabase;
            try
            {
                UserInDatabase = await userRepository.GetUserByEmail(Email);
            }
            catch (Exception)
            {
                throw new LoginFailureException("This account doesn't exist");
            }
            
            bool isVerifiedUser = UserInDatabase.Password == Password;

            if (!isVerifiedUser)
            {
                throw new LoginFailureException("Wrong password");
            }

            return mapper.Map<UserDto>(UserInDatabase);
        }
    }
}
