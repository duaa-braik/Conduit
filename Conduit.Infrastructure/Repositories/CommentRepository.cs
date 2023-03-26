using Conduit.Domain.Entities;
using Conduit.Domain.Interfaces;

namespace Conduit.Infrastructure.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ConduitDbContext context) : base(context) { }
    }
}
