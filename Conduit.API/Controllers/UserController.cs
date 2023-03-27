using Conduit.API.Authentication;
using Conduit.API.Authentication.Service;
using Conduit.Application.Interfaces;
using Conduit.Domain.DTOs;
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
            var User = await userService.Register(userInfo);

            token = tokenGenerator.Generate(userInfo, JwtSettings);
            User.Token = token;

            return CreatedAtRoute("UsersAuthentication", User);
        }
    }
}
