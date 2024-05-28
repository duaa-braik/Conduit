using Conduit.Domain.Entities;
using Conduit.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Infrastructure.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ConduitDbContext context) : base(context) { }

        public async Task<Comment> GetCommentById(int Id)
        {
            return await context.Comments.Include(c => c.User).FirstAsync(c => c.CommentId == Id);
        }

        public async Task<List<Comment>> GetComments(Article article)
        {
            return await context.Comments.Where(c => c.Article == article).ToListAsync();
        }
    }
}
