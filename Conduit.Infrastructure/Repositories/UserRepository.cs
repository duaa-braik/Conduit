using Conduit.Domain.Entities;
using Conduit.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ConduitDbContext context) : base(context) { }

        public async Task<User> GetUserByEmail(string email)
        {
            return await context.User.FirstAsync(user => user.Email == email);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await context.User.FirstAsync(user => user.Username == username);
        }

        public async Task<User> GetUserWithFollowings(string Username)
        {
            return await context.User
                .Include(user => user.Followings)
                .FirstAsync(user => user.Username == Username);
        }
    }
}
