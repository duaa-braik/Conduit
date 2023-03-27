using Conduit.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        
    }
}
