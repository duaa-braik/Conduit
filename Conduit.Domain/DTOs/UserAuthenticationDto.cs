namespace Conduit.Application.Services
{
    public class UserAuthenticationDto
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Bio { get; set; }
        public string Token { get; set; }
    }
}