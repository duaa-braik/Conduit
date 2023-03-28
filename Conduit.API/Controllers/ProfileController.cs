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

        [HttpPost]
        [Authorize]
        [Route("{Username}/follow")]
        public async Task<ActionResult> FollowUser(string Username)
        {
            var CurrentUserName = User.Claims.FirstOrDefault(claim => claim.Type == "UserName")!.Value;

            if (Username == CurrentUserName) return BadRequest();

            var FollowedUser = await profileService.FollowUser(Username, CurrentUserName);

            return Ok(FollowedUser);
        }

    }
}
