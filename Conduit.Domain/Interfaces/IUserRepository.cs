using Conduit.Domain.Entities;

namespace Conduit.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByUsername(string username);
    }
}
