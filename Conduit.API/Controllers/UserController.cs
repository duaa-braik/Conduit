using Conduit.API.Authentication;
using Conduit.API.Authentication.Service;
using Conduit.Application.Interfaces;
using Conduit.Domain.DTOs;
using Conduit.Domain.Exceptions;
using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("/api/users", Name = "UsersAuthentication")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ITokenGenerator tokenGenerator;
        private readonly JwtSettings JwtSettings;
        private string token;

        public UserController
            (IUserService userService, IConfiguration configuration, ITokenGenerator tokenGenerator)
        {
            this.userService = userService;
            this.tokenGenerator = tokenGenerator;
            JwtSettings = new JwtSettings();
            configuration.Bind("JwtSettings", JwtSettings);
        }

        [HttpPost]
        public async Task<ActionResult<UserAuthenticationDto>> Register(UserDto userInfo)
        {
            try
            {
                var User = await userService.Register(userInfo);

                token = tokenGenerator.Generate(userInfo, JwtSettings);
                User.Token = token;

                return CreatedAtRoute("UsersAuthentication", User);
            }
            catch (UniqueConstraintException)
            {
                return UnprocessableEntity();
            }
            
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<UserAuthenticationDto>> Login(UserLoginDto userInfo)
        {
            try
            {
                var UserDto = await userService.Login(userInfo);

                token = tokenGenerator.Generate(UserDto, JwtSettings);

                var User = userService.MapToUserAuthenticationDto(UserDto);
                User.Token = token;

                return Ok(User);
            }
            catch (AccountDoesNotExistException)
            {
                return Unauthorized();
            }
            catch (WrongPasswordException)
            {
                return Unauthorized();
            }
        }
    }
}
