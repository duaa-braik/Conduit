using Conduit.Domain.Entities;
using Conduit.Domain.Interfaces;

namespace Conduit.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ConduitDbContext context) : base(context) { }
    }
}
