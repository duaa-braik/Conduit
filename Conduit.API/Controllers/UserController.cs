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
    [Route("/api")]
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
        [AllowAnonymous]
        [Route("users", Name = "UsersAuthentication")]
        public async Task<ActionResult<UserAuthenticationDto>> Register(UserDto userInfo)
        {
            var User = await userService.Register(userInfo);

            token = tokenGenerator.Generate(userInfo, JwtSettings);
            User.Token = token;

            return CreatedAtRoute("UsersAuthentication", User);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("users/login")]
        public async Task<ActionResult<UserAuthenticationDto>> Login(UserLoginDto userInfo)
        {
            var UserDto = await userService.Login(userInfo);

            token = tokenGenerator.Generate(UserDto, JwtSettings);

            var User = userService.MapToUserAuthenticationDto(UserDto);
            User.Token = token;

            return Ok(User);
        }

        [HttpGet]
        [Authorize]
        [Route("user")]
        public async Task<ActionResult<UserAuthenticationDto>> GetCurrentUser()
        {
            var CurrentUserEmail = User.Claims.First(claim => claim.Type == "Email").Value;

            var CurrentUser = await userService.GetCurrentUser(CurrentUserEmail);

            token = Request.Headers.Authorization.ToString().Split(" ").Last();
            CurrentUser.Token = token;

            return Ok(CurrentUser);
            
        }

        [HttpPut]
        [Authorize]
        [Route("user")]
        public async Task<ActionResult<UserAuthenticationDto>> UpdateUser(UserUpdateDto userUpdates)
        {
            var CurrentUserEmail = User.Claims.First(claim => claim.Type == "Email").Value;

            var UserAfterUpdates = await userService.UpdateUser(userUpdates, CurrentUserEmail);

            return Ok(UserAfterUpdates);

        }
    }
}
