using Conduit.Domain.Entities;
using Conduit.Domain.Interfaces;

namespace Conduit.Infrastructure.Repositories
{
    public class ArticleRepository : GenericRepository<Article>, IArticleRepository
    {
        public ArticleRepository(ConduitDbContext context) : base(context) { }
    }
}
