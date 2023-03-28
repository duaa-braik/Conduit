using Conduit.Application.Interfaces;
using Conduit.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API.Controllers
{
    [ApiController]
    [Route("/api/profiles")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService profileService;

        public ProfileController(IProfileService profileService)
        {
            this.profileService = profileService;
        }

        [HttpGet]
        [Route("{Username}")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile(string Username)
        {
            var UsernameClaim = User.Claims.FirstOrDefault(claim => claim.Type == "UserName");

            UserProfileDto UserProfile;

            if (UsernameClaim == null)
            {
                UserProfile = await profileService.GetUserProfile(Username);
            }
            else
            {
                UserProfile = await profileService.GetUserProfile(Username, UsernameClaim.Value);
            }
            return Ok(UserProfile);
        }

    }
}
