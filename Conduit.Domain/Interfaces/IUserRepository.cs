using Conduit.Domain.Entities;

namespace Conduit.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> FollowUser(User userToFollow, User CurrentUser);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserWithFollowings(string Username);
    }
}
